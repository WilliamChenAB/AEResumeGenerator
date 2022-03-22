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
		public async Task<IActionResult> NewWorkspace(string division, int proposalNumber, string name, int EID)
		{

			WorkspaceEntity entity = new WorkspaceEntity
			{
				Division = division,
				Creation_Date = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
				Proposal_Number = proposalNumber,
				Name = name,
				EID = EID
			};

			_databaseContext.Workspace.Add(entity);
			await _databaseContext.SaveChangesAsync();
			var WID = _databaseContext.Workspace.OrderBy(x => x.WID).Last();

			return CreatedAtAction(
				nameof(GetWorkspace),
				new { WID =  WID},
				entity);
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

			// Get all resumes in workspace
			result.Resumes = (from r in _databaseContext.Resume
							 where r.WID == WID
							 select ControllerHelpers.ResumeEntityToModel(r))
							 .ToList();

			// Get all sectors in resumes
			result.Resumes.ForEach(r => r.SectorList = (from s in _databaseContext.Sector
													   where r.RID == s.RID
													   select ControllerHelpers.SectorEntityToModel(s))
													   .ToList());

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

			if (resume == null)
			{
				return NotFound();
			}
			resume.Status = "Submitted";
						

			// Create a new Resume with the same sectors but new SID and add to Workspace
			ResumeEntity entity = new ResumeEntity
			{
				Creation_Date = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
				Last_Edited = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
				EID = resume.EID,
				EmployeeName = resume.EmployeeName,
				Status = Status.Submitted.ToString(),
				TemplateID = resume.TemplateID,
				WID = WID,
				Name = $"Copy of {resume.Name}",
				TemplateName = resume.TemplateName
			};


			// Add copy resume to db			
			var resultResume = _databaseContext.Resume.AddAsync(entity).Result;
			await _databaseContext.SaveChangesAsync();


			// Copy all of the sectors from the old resume to add to the Sector table
			var sectors = _databaseContext.Sector.Where(sector => sector.RID == resume.RID);			

			sectors.ToList().ForEach(s => _databaseContext.Sector.Add(new SectorEntity
			{
				Content = s.Content,
				EID = s.EID,
				Creation_Date = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
				TypeID = s.TypeID,
				TypeTitle = s.TypeTitle,
				Last_Edited = s.Last_Edited,
				RID = resultResume.Entity.RID,
				Image = s.Image,
				Division = s.Division,
				ResumeName = resultResume.Entity.Name				
			}));

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(resultResume);
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

			var workspace = await _databaseContext.Workspace.FindAsync(WID); ;

			if(workspace == null)
            {
				return NotFound();
            }

			// Create a blank resume that has all the sectors in the template
			var template = await _databaseContext.Resume_Template.FindAsync(TemplateID);
            if (template == null)
            {
				return NotFound();
            }
			var templateModel = ControllerHelpers.TemplateEntityToModel(template);



			ResumeEntity templateResume = new ResumeEntity();
			templateResume.TemplateID = TemplateID;
			templateResume.Status = Status.Requested.ToString();
			templateResume.EID = EID;
			templateResume.WID = WID;
			templateResume.TemplateName = template.Title;
			templateResume.Creation_Date = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
			templateResume.Last_Edited = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
			templateResume.Name = $"Resume Request for {workspace.Name}: Employee {employee.Name}";
			templateResume.EmployeeName = employee.Name;

			// Get all sector types for the template
			templateModel.SectorTypes = (from t in _databaseContext.Template_Type
										join s in _databaseContext.SectorType on t.TypeID equals s.TypeID
										where t.TemplateID == TemplateID
										select ControllerHelpers.SectorTypeEntityToModel(s))
										.ToList();

			var resultResume =  _databaseContext.Resume.AddAsync(templateResume).Result;
			await _databaseContext.SaveChangesAsync();			

			// Add the sectors to the sector table and assign to created resume
			foreach (var sectorType in templateModel.SectorTypes)
			{							
				_databaseContext.Sector.Add(new SectorEntity
				{
					Creation_Date = DateTime.Now.ToString("yyyMMdd HH:mm:ss"),
					Last_Edited = DateTime.Now.ToString("yyyMMdd HH:mm:ss"),
					Content = "",
					EID = EID,
					TypeID = sectorType.TypeID,
					TypeTitle = sectorType.Title,
					RID = resultResume.Entity.RID,
					ResumeName = $"Resume Request for {workspace.Name}: Employee {employee.Name}",
					Division = workspace.Division,
					Image = ""
				});				
			}
			
			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return BadRequest(ex.Message);
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

			var employee = await _databaseContext.Employee.FindAsync(EID);

			if(employee == null)
            {
				return NotFound();
            }

			ResumeEntity entity = new ResumeEntity();
			entity.EID = EID;
			entity.Status = Status.InProgress.ToString();
			entity.WID = WID;
			entity.Last_Edited = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
			entity.Creation_Date = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
			entity.Name = name;
			entity.EmployeeName = employee.Name;
			entity.TemplateID = 0;
			entity.TemplateName = "";

			_databaseContext.Resume.Add(entity);


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

