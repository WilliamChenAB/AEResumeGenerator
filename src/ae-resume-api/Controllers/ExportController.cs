using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

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

						
			// Create a file to write to
			string path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "resumes.txt");
			using (StreamWriter sw = System.IO.File.CreateText(path))
			{
				sw.WriteLine(JsonSerializer.Serialize(result));
			}
			var text = JsonSerializer.Serialize(result);
			var zipFileMemoryStream = new MemoryStream();			
			using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
			{
					var entry = archive.CreateEntry("resumes.txt");
					using (var entryStream = entry.Open())
					using (MemoryStream stringInMemoryStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(text)))
					{
						await stringInMemoryStream.CopyToAsync(entryStream);
					}				
			}

			zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
			return File(zipFileMemoryStream, "application/octet-stream", "resumes.zip");

			//Response.ContentType = "application/octet-stream";
			//Response.Headers.Add("Content-Disposition", "attachment; filename=\"resumes.zip\"");
			//using (ZipArchive archive = new ZipArchive(Response.BodyWriter.AsStream(), ZipArchiveMode.Create))
			//{
			//		var botFileName = Path.GetFileName(path);
			//		var entry = archive.CreateEntry(botFileName);
			//		using (var entryStream = entry.Open())
			//		using (var fileStream = System.IO.File.OpenRead(path))
			//		{
			//			await fileStream.CopyToAsync(entryStream);
			//		}			
			//}			

			//return new JsonResult(result);
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
