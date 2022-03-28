using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.Controllers
{

	[Route("Workspace")]
	[ApiController]
	public class WorkspaceController : Controller
	{

		readonly DatabaseContext _databaseContext;
		private readonly IConfiguration configuration;

		public WorkspaceController(DatabaseContext dbContext, IConfiguration configuration)
		{
			_databaseContext = dbContext;
			this.configuration = configuration;
		}

		private async Task RemoveExistingResumes(WorkspaceEntity workspace, Guid employeeId)
        {
			var existing = workspace.Resumes.FindAll(r => r.EmployeeId == employeeId);
			if (existing != null)
			{
				_databaseContext.Resume.RemoveRange(existing);
				await _databaseContext.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Create a new Workspace
		/// </summary>
		[HttpPost]
		[Route("New")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> New(string division, int proposalNumber, string name)
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();
			var guid = Guid.Parse(EmployeeId);

			WorkspaceEntity entity = new WorkspaceEntity
			{
				Division = division,
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				Proposal_Number = proposalNumber,
				Name = name,
				EmployeeId = guid
			};

			var wkspc = _databaseContext.Workspace.Add(entity);
			await _databaseContext.SaveChangesAsync();
			var WorkspaceId = wkspc.Entity.WorkspaceId;

			return CreatedAtAction(
				nameof(Get),
				new { WorkspaceId = WorkspaceId },
				entity);

		}

		/// <summary>
		/// Get a workspace
		/// </summary>
		[HttpGet]
		[Route("Get")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<WorkspaceModel>> Get(int WorkspaceId)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);
			if (workspace == null) return NotFound("Workspace not found");

			return ControllerHelpers.WorkspaceEntityToModel(workspace);

		}

		/// <summary>
		/// Delete a workspace
		/// </summary>
		[HttpDelete]
		[Route("Delete")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> Delete(int WorkspaceId)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);

			if (workspace == null) return NotFound("Workspace not found");

			_databaseContext.Workspace.Remove(workspace);
			await _databaseContext.SaveChangesAsync();

			return Ok();
		}

		/// <summary>
		/// Get all Workspaces for a PA
		/// </summary>
		[HttpGet]
		[Route("GetPersonal")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<WorkspaceModel>>> GetPersonal()
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();
			var guid = Guid.Parse(EmployeeId);

			var employee = await _databaseContext.Employee.FindAsync(guid);
			List<WorkspaceModel> result = new List<WorkspaceModel>();

			foreach (var workspace in employee.Workspaces)
			{
				result.Add(ControllerHelpers.WorkspaceEntityToModel(workspace));
			}
			return result;

		}


		// ===============================================================================
		// RESUMES
		// ===============================================================================


		/// <summary>
		/// Copy a Resume to a Workspace
		/// </summary>
		[HttpPost]
		[Route("CopyResume")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> CopyResume(int ResumeId, int WorkspaceId)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);
			if (workspace == null) return NotFound("Workspace not found");

			// Update old copy to regular
			var resume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (resume == null) return NotFound("Resume Not found");
			resume.Status = Status.Regular;

			//Check if the employee already has a resume in the workspace and remove it
			await RemoveExistingResumes(workspace, resume.EmployeeId);

			// Create a new Resume with the same sectors but new SectorId and add to Workspace
			ResumeEntity entity = new ResumeEntity
			{
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				EmployeeId = resume.EmployeeId,
				Status = Status.Regular,
				TemplateId = resume.TemplateId,
				WorkspaceId = WorkspaceId,
				Name = $"Copy of {resume.Name}"
			};

			// Add copy resume to db
			var resultResume = _databaseContext.Resume.Add(entity);
			await _databaseContext.SaveChangesAsync();

			// Copy all of the sectors from the old resume to add to the Sector table
			var sectors = _databaseContext.Sector.Where(sector => sector.ResumeId == resume.ResumeId);

			sectors.ToList().ForEach(
                s => _databaseContext.Sector.Add(new SectorEntity
                {
                    Content = s.Content,
                    Creation_Date = ControllerHelpers.CurrentTimeAsString(),
                    TypeId = s.TypeId,
                    Last_Edited = s.Last_Edited,
                    ResumeId = resultResume.Entity.ResumeId,
                    Image = s.Image,
                    Division = s.Division
                }));

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok(resultResume.Entity);
		}

		/// <summary>
		/// Get all Resumes from a Workspace
		/// </summary>
		[HttpGet]
		[Route("GetResumes")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumes(int WorkspaceId)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);


			if (workspace == null)
			{
				return NotFound("Workspace not found");
			}

			//var resumes = workspace.Resumes;
			var resumes = workspace.Resumes.Where(r => r.Status != Status.Exported);
			List<ResumeModel> result = new List<ResumeModel>();
			foreach (var resume in resumes)
			{
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
			}

			return Ok(result);
		}

		/// <summary>
		/// Create a Template request for a specific Employee
		/// </summary>
		[HttpPost]
		[Route("CreateTemplateRequest")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> CreateTemplateRequest(int TemplateId, string EmployeeId, int WorkspaceId)
		{
			var guid = Guid.Parse(EmployeeId);

			// Create a blank resume in the employee with the template type
			var employee = await _databaseContext.Employee.FindAsync(guid);
			if (employee == null) return NotFound("Employee not found");

			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId); ;
			if (workspace == null) return NotFound("Workspace not found");

			// Check if the employee already has a resume in the workspace and remove it
			await RemoveExistingResumes(workspace, guid);

			// Create a blank resume that has all the sectors in the template
			var template = await _databaseContext.Template.FindAsync(TemplateId);
			if (template == null)
			{
				return NotFound("Template not found");
			}
			var templateModel = ControllerHelpers.TemplateEntityToModel(template);



			ResumeEntity templateResume = new ResumeEntity();
			templateResume.TemplateId = TemplateId;
			templateResume.Status = Status.Requested;
			templateResume.EmployeeId = guid;
			templateResume.WorkspaceId = WorkspaceId;
			templateResume.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			templateResume.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			templateResume.Name = $"Resume Request for {workspace.Name}: Employee {employee.Name}";

			// Get all sector types for the template
			templateModel.SectorTypes =
				template.TemplateSectors
				.Select(s => ControllerHelpers.SectorTypeEntityToModel(s.SectorType))
				.ToList();

			var resultResume = await _databaseContext.Resume.AddAsync(templateResume);
			await _databaseContext.SaveChangesAsync();

			// Add the sectors to the sector table and assign to created resume
			foreach (var sectorType in templateModel.SectorTypes)
			{
				_databaseContext.Sector.Add(new SectorEntity
				{
					Creation_Date = ControllerHelpers.CurrentTimeAsString(),
					Last_Edited = ControllerHelpers.CurrentTimeAsString(),
					Content = "",
					TypeId = sectorType.TypeId,
					ResumeId = resultResume.Entity.ResumeId,
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
		[Route("AddEmptyResume")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> AddEmptyResume(int WorkspaceId, int TemplateId, string resumeName)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);
			if (workspace == null) return NotFound("Workspace not found");

			var guid = workspace.EmployeeId;

			var employee = await _databaseContext.Employee.FindAsync(guid);
			if (employee == null) return NotFound("Employee not found");

			//Check if the employee already has a resume in the workspace and remove it
			await RemoveExistingResumes(workspace, guid);

			ResumeEntity entity = new ResumeEntity();
			entity.EmployeeId = guid;
			entity.Status = Status.Regular;
			entity.WorkspaceId = WorkspaceId;
			entity.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			entity.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			entity.Name = resumeName;
			entity.TemplateId = TemplateId;

			// Get the template
			var template = await _databaseContext.Template.FindAsync(TemplateId);
			if (template == null)
			{
				return NotFound("Template not found");
			}
			var templateModel = ControllerHelpers.TemplateEntityToModel(template);
			// Get all sector types for the template
			templateModel.SectorTypes =
				template.TemplateSectors
				.Select(s => ControllerHelpers.SectorTypeEntityToModel(s.SectorType))
				.ToList();

			var resultResume = _databaseContext.Resume.AddAsync(entity).Result;
			await _databaseContext.SaveChangesAsync();

			// Add the sectors to the sector table and assign to created resume
			foreach (var sectorType in templateModel.SectorTypes)
			{
				_databaseContext.Sector.Add(new SectorEntity
				{
					Creation_Date = ControllerHelpers.CurrentTimeAsString(),
					Last_Edited = ControllerHelpers.CurrentTimeAsString(),
					Content = "",
					TypeId = sectorType.TypeId,
					ResumeId = resultResume.Entity.ResumeId,
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
				return BadRequest(ex.Message);
			}

			return Ok(entity);
		}

		[HttpPost]
		[Route("SubmitResume")]
		public async Task<IActionResult> SubmitResume(int ResumeId, int WorkspaceId)
		{
			var resume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (resume == null) return NotFound("Resume not found");

			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();
			var guid = Guid.Parse(EmployeeId);

			//Check if the employee already has a resume in the workspace and remove it
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);
			if (workspace == null) return NotFound();
			await RemoveExistingResumes(workspace, guid);

			// Assign status to regular and create copy to be stored in the workspace
			resume.Status = Status.Regular;

			var workspaceResume = new ResumeEntity();
			workspaceResume.WorkspaceId = WorkspaceId;
			workspaceResume.EmployeeId = guid;
			workspaceResume.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			workspaceResume.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			workspaceResume.Status = Status.Regular;
			workspaceResume.Name = resume.Name;
			workspaceResume.TemplateId = resume.TemplateId;

			await _databaseContext.Resume.AddAsync(workspaceResume);

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound("CAUGHT EXCEPTION: " + ex.Message);
			}

			return Ok(workspaceResume);

		}


		// ===============================================================================
		// MISC
		// ===============================================================================

		[HttpGet]
		[Route("GetAllSectorTypes")]
		[Authorize(Policy = "PA")]
		public async Task<IEnumerable<SectorTypeModel>> GetAllSectorTypes(int WorkspaceId)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);
			if (workspace == null) NotFound("workspace not found");

			var sectorTypes = await (from r in _databaseContext.Resume
									 join s in _databaseContext.Sector on r.ResumeId equals s.ResumeId
									 join st in _databaseContext.SectorType on s.TypeId equals st.TypeId
									 where r.WorkspaceId == WorkspaceId
									 select ControllerHelpers.SectorTypeEntityToModel(st))
							  .ToListAsync();

			return sectorTypes;

		}


	}

}