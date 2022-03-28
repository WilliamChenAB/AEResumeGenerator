using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ae_resume_api.Controllers
{

    [Route("Resume")]
    [ApiController]
    public class ResumeController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public ResumeController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }

		/// <summary>
		/// Add a Resume to personal resumes
		/// </summary>
		[HttpPost]
		[Route("NewPersonal")]
		public async Task<ActionResult<ResumeModel>> NewResume(int TemplateId, string resumeName)
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();
			return await New(TemplateId, resumeName, EmployeeId);
		}

		/// <summary>
		/// Add a Resume to an Employee
		/// </summary>
		[HttpPost]
		[Route("New")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<ResumeModel>> New(int TemplateId, string resumeName, string EmployeeId)
		{
			var guid = Guid.Parse(EmployeeId);

			// Find the template record
			var template = await _databaseContext.Template.FindAsync(TemplateId);
			if (template == null) return NotFound("Template not found");

			var employee = await _databaseContext.Employee.FindAsync(guid);
			if (employee == null) return NotFound("Employee not found");

			// Find the Sector Types associated with that template
			var sectorTypes = template.TemplateSectors.Select(x => x.SectorType).ToList();

			ResumeEntity entity = new ResumeEntity
			{
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				EmployeeId = guid,
				TemplateId = TemplateId,
				Name = resumeName,
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				Status = Status.Regular
			};

			var resume = _databaseContext.Resume.Add(entity);

			foreach (var sector in sectorTypes)
			{
				_databaseContext.Sector.Add(new SectorEntity
				{
					Creation_Date = ControllerHelpers.CurrentTimeAsString(),
					Last_Edited = ControllerHelpers.CurrentTimeAsString(),
					Content = "",
					TypeId = sector.TypeId,
					ResumeId = resume.Entity.ResumeId,
					Division = "",
					Image = ""
				});
			}


			await _databaseContext.SaveChangesAsync();

			return CreatedAtAction(
				nameof(Get),
				new { ResumeId = resume.Entity.ResumeId },
				 resume.Entity);
		}


		/// <summary>
		/// Delete a Resume
		/// </summary>
		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete(int ResumeId)
		{
			var resume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (resume == null) return NotFound("Resume not found");

			_databaseContext.Resume.Remove(resume);
			await _databaseContext.SaveChangesAsync();

			return Ok();
		}

		/// <summary>
		/// Get a Resume
		/// </summary>
		[HttpGet]
		[Route("Get")]
		public async Task<ActionResult<ResumeModel>> Get(int ResumeId)
		{
			var resume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (resume == null) return NotFound("Resume not found");

			return ControllerHelpers.ResumeEntityToModel(resume); ;
		}

		/// <summary>
		/// Get all Resumes for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllForEmployee")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetAllForEmployee(string EmployeeId)
		{
			var resumes = _databaseContext.Resume.Where(r => r.EmployeeId == Guid.Parse(EmployeeId));
			List<ResumeModel> result = new List<ResumeModel>();

			foreach (var resume in resumes) result.Add(ControllerHelpers.ResumeEntityToModel(resume));

			return result;
		}

		/// <summary>
		/// Get personal Resumes
		/// </summary>
		[HttpGet]
		[Route("GetPersonal")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetPersonal()
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();
			return await GetPersonalForEmployee(EmployeeId);
		}

		/// <summary>
		/// Get personal Resume for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetPersonalForEmployee")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetPersonalForEmployee(string EmployeeId)
		{
			// TODO: add only three statuses for resumes reqested, regular, exported
			var resumes = _databaseContext.Resume.Where(r =>
					r.EmployeeId == Guid.Parse(EmployeeId) &&
					((r.Status == Status.Regular && r.WorkspaceId == null)
					|| r.Status == Status.Requested)
				).ToList();

			if (resumes == null) return NotFound("Resume not found");

			List<ResumeModel> result = new List<ResumeModel>();
			foreach (var resume in resumes)
			{
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
			}

			return result;
		}

	}
}
