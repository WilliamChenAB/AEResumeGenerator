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
		public async Task<IActionResult> New(string division, string proposalNumber, string name)
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

			// Ensure that proposal number is unique
			var proposalExists = await _databaseContext.Workspace.
				AnyAsync(w => w.Proposal_Number == proposalNumber);
            if (proposalExists)
            {
				return BadRequest("Cannot create Workspace with the same Proposal Number");
            }

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

			// Don't delete this - it loads the resumes into memory so that ClientCascade works correctly.
			// We're not using regular Cascading because it causes deletion cycles, so this lets us control the scope of the cascade
			// If you're curious, see https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete?msclkid=cd55484aaf3e11ecbedf860b25799ab7#database-cascade-limitations
			var resumes = workspace.Resumes;

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
			return await AddResumeHelper(TemplateId, EmployeeId, WorkspaceId,
				(wname, ename) => $"Resume Request for {wname}: Employee {ename}",
				Status.Requested);
		}

		[HttpPost]
		[Route("AddEmptyResume")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> AddEmptyResume(int WorkspaceId, int TemplateId, string resumeName, string EmployeeId)
		{
			return await AddResumeHelper(TemplateId, EmployeeId, WorkspaceId, (_, _) => resumeName, Status.Regular);
		}

		private async Task<IActionResult> AddResumeHelper(int TemplateId, string EmployeeId, int WorkspaceId, Func<string,string,string> nameGen, Status status)
		{
			if (EmployeeId == null) return NotFound("Employee not found");
			var guid = Guid.Parse(EmployeeId);

			var employee = await _databaseContext.Employee.FindAsync(guid);
			if (employee == null) return NotFound("Employee not found");

			var template = await _databaseContext.Template.FindAsync(TemplateId);
			if (template == null) return NotFound("Template not found");

			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId); ;
			if (workspace == null) return NotFound("Workspace not found");

			// Check if the employee already has a resume in the workspace and remove it
			await RemoveExistingResumes(workspace, guid);

			// Create a blank resume that has all the sectors in the template
			ResumeEntity entity = new ResumeEntity
			{
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				EmployeeId = guid,
				TemplateId = TemplateId,
				WorkspaceId = WorkspaceId,
				Name = nameGen(workspace.Name, employee.Name),
				Status = status
			};

			var resume = _databaseContext.Resume.Add(entity);
			await _databaseContext.SaveChangesAsync();

			await ControllerHelpers.PopulateTemplateSectors(template, resume.Entity.ResumeId, _databaseContext);

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

			var resume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (resume == null) return NotFound("Resume Not found");
			if (resume.WorkspaceId == WorkspaceId) return BadRequest("Cannot copy a resume to the workspace it resides in");

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
			await CopySectorsHelper(resume, resultResume.Entity);

			return Ok(resultResume.Entity);
		}


		[HttpPost]
		[Route("SubmitResume")]
		public async Task<IActionResult> SubmitResume(int ResumeId)
		{
			var workspaceResume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (workspaceResume == null) return NotFound("Resume not found");
			if (workspaceResume.WorkspaceId == null) return NotFound("Workspace not found");

			// Assign status to regular and create copy to be stored in the employee
			workspaceResume.Status = Status.Regular;

			var entity = new ResumeEntity()
			{
				EmployeeId = workspaceResume.EmployeeId,
				TemplateId = workspaceResume.TemplateId,
				WorkspaceId = null,
				Last_Edited = workspaceResume.Last_Edited,
				Creation_Date = workspaceResume.Creation_Date,
				Status = Status.Regular,
				Name = workspaceResume.Name
			};

			var employeeResume = _databaseContext.Resume.Add(entity);
			await _databaseContext.SaveChangesAsync();

			await CopySectorsHelper(workspaceResume, employeeResume.Entity);

			return Ok(workspaceResume);

		}

		private async Task CopySectorsHelper(ResumeEntity source, ResumeEntity dest)
        {
			var sectors = source.Sectors;

			sectors.ForEach(
				s => _databaseContext.Sector.Add(new SectorEntity
				{
					Content = s.Content,
					Creation_Date = ControllerHelpers.CurrentTimeAsString(),
					TypeId = s.TypeId,
					Last_Edited = s.Last_Edited,
					ResumeId = dest.ResumeId,
					Image = s.Image,
					Division = s.Division
				}));

			await _databaseContext.SaveChangesAsync();

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