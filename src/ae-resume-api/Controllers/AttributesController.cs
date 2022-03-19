using System;
using ae_resume_api.Attributes;
using ae_resume_api.Facade;
using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ae_resume_api.Controllers
{
	[Route("Attributes")]
	[ApiController]
	public class AtrributesController : ControllerBase
	{
		readonly DatabaseContext _databaseContext;
		


		public AtrributesController(DatabaseContext dbContext)
		{
			_databaseContext = dbContext;
			
		}

		/// <summary>
		/// Create a new Workspace
		/// </summary>
		[HttpPost]
		[Route("NewWorkspace")]
		public async Task<IActionResult> NewWorkspace([FromBody] WorkspaceModel model)
		{

			WorkspaceEntity entity = new WorkspaceEntity
			{
				WID = model.WID,
				Division = model.Division,
				Creation_Date = DateTime.Now.ToString("yyyMMdd"),
				Proposal_Number = model.ProposalNumber,
				Name = model.Name
			};
			
			_databaseContext.Workspace.Add(entity);
			await _databaseContext.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetWorkspace),
				new { WID = model.WID },
				model);
		}

		/// <summary>
		/// Get a workspace
		/// </summary>
		[HttpGet]
		[Route("GetWorkspace")]
		public async Task<ActionResult<WorkspaceModel>> GetWorkspace(int WID)
		{
			// var workspace = Workspaces.Find(x => x.WID == WID);

			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if (workspace == null)
			{
				return NotFound();
			}

			WorkspaceModel result = ControllerHelpers.WorkspaceEntityToModel(workspace);			
			

			return result;

		}

		/// <summary>
		/// Copy a Resume to a Workspace
		/// </summary>
		[HttpPost]
		[Route("CopyResume")]
		public async Task<IActionResult> CopyResume(int RID, int WID)
		{
			// var workspace = Workspaces.Find(x => x.WID == WID);			
			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if (workspace == null)
			{
				return NotFound();
			}


			// Update old copy to submitted
			var resume = await _databaseContext.Resume.FindAsync(RID);
			resume.Status = "Submitted";
			resume.WID = WID;
			// var resume = Workspaces.First().Resumes.Last();

			if (resume == null)
			{
				return NotFound();
			}

			//resume.WID = workspace.WID;			
			//workspace.Resumes.Add(resume);

			// Create a new Resume with the same sectors but new SID and add to Workspace
			ResumeEntity entity = new ResumeEntity
			{
				Creation_Date = DateTime.Now.ToString("yyyMMdd"),
				EID = resume.EID,
				TemplateID = resume.TemplateID,
				Status = resume.Status,
				Last_Edited = resume.Last_Edited,
				WID = WID
			};

			_databaseContext.Resume.Add(entity);			
			await _databaseContext.SaveChangesAsync();


			// Copy all of the sectors from the old resume to add to the Sector table
			var sectors = _databaseContext.Sector.Where(sector => sector.RID == resume.RID);
			var newResume = await _databaseContext.Resume.FindAsync(entity);

			sectors.ToList().ForEach(s => _databaseContext.Sector.Add(new SectorEntity
			{
				Content = s.Content,
				EID = s.EID,
				Creation_Date = DateTime.Now.ToString("yyyMMdd"),
				TypeID = s.TypeID,
				TypeTitle = s.TypeTitle,
				Last_Edited = s.Last_Edited,
				RID = newResume.RID
			}));

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(resume);


		}

		/// <summary>
		/// Delete a workspace
		/// </summary>
		[HttpDelete]
		[Route("DeleteWorkspace")]
		public async Task<IActionResult> DeleteWorkspace(int WID)
		{
			//var workspace = Workspaces.Find(x => x.WID == WID);

			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if (workspace == null)
			{
				return NotFound();
			}

			//Workspaces.Remove(workspace);

			_databaseContext.Workspace.Remove(workspace);
			await _databaseContext.SaveChangesAsync();

			return Ok();
		}


		/// <summary>
		/// Get all Resumes from a Workspace
		/// </summary>
		[HttpGet]
		[Route("GetResumesForWorkspace")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumesForWorkspace(int WID)
		{
			//var workspace = Workspaces.Find(x => x.WID == WID);

			var workspace = await _databaseContext.Workspace.FindAsync(WID);


			if (workspace == null)
			{
				return NotFound();
			}

			//var resumes = workspace.Resumes;
			var resumes = _databaseContext.Resume.Where(r => r.WID == workspace.WID);
			List<ResumeModel> result = new List<ResumeModel>();
			foreach (var resume in resumes)
			{
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
			}

			return Ok(result);

		}
		/// <summary>
		/// Get all Workspaces or a PA
		/// </summary>
		[HttpGet]
		[Route("GetAllWorkspacesForEmployee")]
		public IEnumerable<WorkspaceModel> GetAllWorkspacesForEmployee(int EID)
        {
			var workspaces = _databaseContext.Workspace.Where(w => w.EID == EID);
			List<WorkspaceModel> result = new List<WorkspaceModel>();
            foreach (var workspace in workspaces)
            {
                result.Add(ControllerHelpers.WorkspaceEntityToModel(workspace));
            }
            return result;

        }

		/// <summary>
		/// Create a Template request for a specific Employee
		/// </summary>
		[HttpPost]
		[Route("CreateTemplateRequest")]
		public async Task<IActionResult> CreateTemplateRequest(int TemplateID, int EID, int WID)
		{

			// Create a blank resume in the employee with the template type
			var employee = await _databaseContext.Employee.FindAsync(EID);

			if (employee == null)
			{
				return NotFound();
			}

			// Create a blank resume that has all the sectors in the template
			var template = ControllerHelpers.TemplateEntityToModel(await _databaseContext.Resume_Template.FindAsync(TemplateID));
			

			if (template == null)
			{
				return NotFound();
			}
			// TODO: I dont think we can just loop through all the sectortypes depending on how we are storing templates
			ResumeEntity templateResume = new ResumeEntity();
			templateResume.TemplateID = TemplateID;
			templateResume.Status = Status.Requested.ToString();
			templateResume.EID = EID;		
			templateResume.WID = WID;
			foreach (var sectorType in template.SectorTypes)
			{
				SectorModel sector = new SectorModel();
				sector.SectorType = sectorType.TypeID;
				sector.CreationDate = DateTime.Now;

				_databaseContext.Template_Type.Add(new TemplateSectorsEntity
				{
					TemplateID = TemplateID,
					TypeID = sectorType.TypeID
				});

				await _databaseContext.Resume.AddAsync(templateResume);
				
			}
			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(employee);
		}

		[HttpPost]
		[Route("AddEmptyResumeToWorkspace")]
		public async Task<IActionResult> AddEmptyResumeToWorkspace(int WID, int EID, string name)
        {
			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if(workspace == null)
            {
				return NotFound();
            }

			ResumeEntity entity = new ResumeEntity();
			entity.EID = EID;
			entity.Status = Status.InProgress.ToString();
			entity.WID = WID;
			entity.Last_Edited = DateTime.Now.ToString("yyyMMdd");
			entity.Creation_Date = DateTime.Now.ToString("yyyMMdd");
			entity.Name = name;


			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(entity);
        }
		
		

	}
}

