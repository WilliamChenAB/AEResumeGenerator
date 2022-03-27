using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.Controllers
{

	[Route("Export")]
	[ApiController]
	public class ExportController : Controller
	{

		readonly DatabaseContext _databaseContext;
		private readonly IConfiguration configuration;

		public ExportController(DatabaseContext dbContext, IConfiguration configuration)
		{
			_databaseContext = dbContext;
			this.configuration = configuration;
		}

		/// <summary>
		/// Export Resume
		/// </summary>
		[HttpGet]
		[Route("Resume")]
		public async Task<IActionResult> ExportResume(int ResumeId)
		{
			var resume = await _databaseContext.Resume.FindAsync(ResumeId);
			if (resume == null) return NotFound("Resume not found");

			var result = ControllerHelpers.ResumeEntityToModel(resume);

			return new JsonResult(result);
		}

		/// <summary>
		/// Export Resumes in Workspace
		/// </summary>
		[HttpGet]
		[Route("ResumesInWorkspace")]
		public async Task<IActionResult> ExportResumesInWorkspace(int WorkspaceId)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WorkspaceId);

			if (workspace == null)
			{
				return NotFound("Workspace not found");
			}

			var resumes = await (from r in _databaseContext.Resume
								 where r.WorkspaceId == WorkspaceId
								 select r).ToListAsync();

			// Get all sectors in resume and set status as exported
			// Employees cannot use exported resumes

			List<ResumeModel> result = new List<ResumeModel>();
			foreach (var resume in resumes)
			{
				resume.Status = Status.Exported;
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
			}

			await _databaseContext.SaveChangesAsync();

			// TODO: zip output
			return new JsonResult(result);

		}

		/// <summary>
		/// Export Resumes in Workspace
		/// </summary>
		[HttpGet]
		[Route("ResumesInWorkspaceXML.xml")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> ExportResumesInWorkspaceXML(int WorkspaceId)
		{
			throw new NotImplementedException();
		}

	}
}
