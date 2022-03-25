using ae_resume_api.Attributes;
using ae_resume_api.Facade;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ae_resume_api.Admin;

namespace ae_resume_api.Controllers
{
    [Route("Attributes")]
	[ApiController]
	public class AttributesController : ControllerBase
	{
		readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public AttributesController(DatabaseContext dbContext, IConfiguration configuration)
		{
			_databaseContext = dbContext;
            this.configuration = configuration;
        }

		/// <summary>
		/// Create a new Workspace
		/// </summary>
		[HttpPost]
		[Route("NewWorkspace")]
		[Authorize (Policy = "PA")]
		public async Task<IActionResult> NewWorkspace(string division, int proposalNumber, string name)
		{
			var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EID == null) return NotFound();

			WorkspaceEntity entity = new WorkspaceEntity
			{
				Division = division,
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
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
		[Authorize(Policy = "PA")]
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
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> CopyResume(int RID, int WID)
		{
			// var workspace = Workspaces.Find(x => x.WID == WID);
			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if (workspace == null)
			{
				return NotFound("Workspace not found");
			}


			// Update old copy to regular
			var resume = await _databaseContext.Resume.FindAsync(RID);

			if (resume == null)
			{
				return NotFound("Resume Not found");
			}
			resume.Status = Status.Regular.ToString();

			//Check if the employee already has a resume in the workspace and remove it
			bool exists = await _databaseContext.Resume.AnyAsync(r => r.WID == WID &&
													 r.EID == resume.EID);
			if (exists)
            {
				_databaseContext.Resume.Where(r => r.WID == WID &&
												   r.EID == resume.EID)
					.ToList().ForEach(r => _databaseContext.Resume.Remove(r));
				await _databaseContext.SaveChangesAsync();
            }



			// Create a new Resume with the same sectors but new SID and add to Workspace
			ResumeEntity entity = new ResumeEntity
			{
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				EID = resume.EID,
				EmployeeName = resume.EmployeeName,
				Status = Status.Requested.ToString(),
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

			sectors.ToList().ForEach(
				async s => await _databaseContext.Sector.AddAsync(new SectorEntity
			{
				Content = s.Content,
				EID = s.EID,
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
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

			return Ok(resultResume.Entity);
		}

		/// <summary>
		/// Delete a workspace
		/// </summary>
		[HttpDelete]
		[Route("DeleteWorkspace")]
		[Authorize(Policy = "PA")]
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
		[Authorize(Policy = "PA")]
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
		[Route("GetAllWorkspaces")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<WorkspaceModel>>> GetAllWorkspaces()
        {
			var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EID == null) return NotFound();

			var workspaces = await _databaseContext.Workspace.Where(w => w.EID == EID).ToListAsync();
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
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> CreateTemplateRequest(int TemplateID, string EID, int WID)
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

			//Check if the employee already has a resume in the workspace and remove it
			bool exists = await _databaseContext.Resume.AnyAsync(r => r.WID == WID &&
													 r.EID == EID);
			if (exists)
			{
				_databaseContext.Resume.Where(r => r.WID == WID &&
												   r.EID == EID)
					.ToList().ForEach(r => _databaseContext.Resume.Remove(r));
				await _databaseContext.SaveChangesAsync();
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
			templateResume.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			templateResume.Last_Edited = ControllerHelpers.CurrentTimeAsString();
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
					Creation_Date = ControllerHelpers.CurrentTimeAsString(),
					Last_Edited = ControllerHelpers.CurrentTimeAsString(),
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
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> AddEmptyResumeToWorkspace(int WID, int TID, string resumeName, string EID)
        {
			// I dont think we can pull EID from token cause it will always be the PA
			//var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			//if (EID == null) return NotFound();

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

			//Check if the employee already has a resume in the workspace and remove it
			bool existsResume = await _databaseContext.Resume.AnyAsync(r => r.WID == WID &&
													 r.EID == EID);
			if (existsResume)
			{
				_databaseContext.Resume.Where(r => r.WID == WID &&
												   r.EID == EID)
					.ToList().ForEach(r => _databaseContext.Resume.Remove(r));
				await _databaseContext.SaveChangesAsync();
			}

			ResumeEntity entity = new ResumeEntity();
			entity.EID = EID;
			entity.Status = Status.Requested.ToString();
			entity.WID = WID;
			entity.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			entity.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			entity.Name = resumeName;
			entity.EmployeeName = employee.Name;
			entity.TemplateID = TID;
			entity.TemplateName = "";

			await _databaseContext.Resume.AddAsync(entity);


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
		[HttpGet]
		[Route("GetAllSectorTypesInWorkspace")]
		[Authorize(Policy = "PA")]
		public async Task<IEnumerable<SectorTypeModel>> GetAllSectorTypesInWorkspace(int WID)
        {
			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if(workspace == null)
            {
				NotFound();
            }

			var sectorTypes = await (from r in _databaseContext.Resume
							   join s in _databaseContext.Sector on r.RID equals s.RID
							   join st in _databaseContext.SectorType on s.TypeID equals st.TypeID
							   where r.WID == WID
							   select ControllerHelpers.SectorTypeEntityToModel(st))
							  .ToListAsync();

			return sectorTypes;

        }
	}
}

