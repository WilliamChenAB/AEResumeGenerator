using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.Controllers
{

    [Route("Search")]
    [ApiController]
    public class SearchController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public SearchController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }

		/// <summary>
		/// Search all Resumes
		/// </summary>
		[HttpGet]
		[Route("Resumes")]
		public async Task<IActionResult> SearchResume(string filter)
		{
			return BadRequest("Not implemented");
		}

		/// <summary>
		/// Search all Sectors
		/// </summary>
		[HttpGet]
		[Route("Sectors")]
		public async Task<IActionResult> SearchSectors(string filter)
		{
			return BadRequest("Not implemented");
		}


		/// <summary>
		/// Search all Employees
		/// </summary>
		[HttpGet]
		[Route("Employees")]
		[Authorize(Policy = "PA")]
		public IEnumerable<EmployeeModel> SearchEmployees(string? filter)
		{
			// Ensure that null value returns all Employees
			if (filter == null)
			{
				filter = "";
			}

			// Current search only supports text fields
			//var employees = _databasecontext.Employee.AsQueryable().
			//	Where(e => e.Name.Contains(filter) ||
			//			   e.Email.Contains(filter)||
			//			   e.Access.Contains(filter)||
			//			   e.Username.Contains(filter));
			// Search all Employees by Employee joined with Resume and Sectors
			var employees = from e in _databaseContext.Employee
							where e.Name.Contains(filter) ||
								  e.Email.Contains(filter) ||
								  e.JobTitle.Contains(filter)
							select new EmployeeModel
							{
								EmployeeId = e.EmployeeId.ToString(),
								Name = e.Name,
								Email = e.Email,
								Access = e.Access,
								JobTitle = e.JobTitle
							};

			return employees;
		}

		/// <summary>
		/// Search all of an Employees resume
		/// </summary>
		[HttpGet]
		[Route("AllEmployeeResumes")]
		[Authorize(Policy = "PA")]
		public async Task<IEnumerable<ResumeModel>> SearchEmployeeResumes(string? filter, string EmployeeId)
		{
			// Ensure that null value returns all Employees
			if (filter == null) filter = "";

			// Get all Resumes for that EmployeeId
			var resumes = from r in _databaseContext.Resume
						  join s in _databaseContext.Sector on r.ResumeId equals s.ResumeId
						  where r.EmployeeId == Guid.Parse(EmployeeId) && (
						  r.Name.Contains(filter) ||
						  r.Creation_Date.Contains(filter) ||
						  r.Last_Edited.Contains(filter) ||
						  (r.Workspace == null ? false : r.Workspace.Proposal_Number.Contains(filter)) ||
						  s.Content.Contains(filter) ||
						  s.Type.Title.Contains(filter) ||
						  s.Division.Contains(filter))
						  select new ResumeModel
						  {
							  EmployeeId = r.EmployeeId.ToString(),
							  CreationDate = ControllerHelpers.parseDate(r.Creation_Date),
							  LastEditedDate = ControllerHelpers.parseDate(r.Last_Edited),
							  ResumeId = r.ResumeId,
							  Status = r.Status,
							  WorkspaceId = r.WorkspaceId,
							  Name = r.Name,
							  TemplateId = r.TemplateId,
							  TemplateName = r.Template.Title
						  };

			return await resumes.Distinct().ToListAsync();

		}

		[HttpGet]
		[Route("EmployeeSectors")]
		[Authorize(Policy = "PA")]
		public IEnumerable<SectorModel> SearchEmployeeSectors(string? filter, string EmployeeId)
		{
			// Clean input filter
			filter = filter ?? "";

			// TODO: test search
			return
				(from s in _databaseContext.Sector
				where s.Resume.EmployeeId == Guid.Parse(EmployeeId) &&
				(s.Content.Contains(filter) ||
				s.Type.Title.Contains(filter) ||
				s.Last_Edited.Contains(filter) ||
				s.Creation_Date.Contains(filter) ||
				s.Resume.Name.Contains(filter) ||
				s.Division.Contains(filter))
				select s)
				.Distinct()
				.ToList()
				.Select(s => ControllerHelpers.SectorEntityToModel(s));

		}

		[HttpGet]
		[Route("OwnSectors")]
		public IEnumerable<SectorModel> SearchEmployeeSectors(string? filter)
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			return SearchEmployeeSectors(filter, EmployeeId);

		}

	}
}
