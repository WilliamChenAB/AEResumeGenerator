using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using ae_resume_api.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Xml.Serialization;

namespace ae_resume_api.Controllers
{
    [Route("Facade")]
	[ApiController]
	public class FacadeController : ControllerBase
	{
		readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public FacadeController(DatabaseContext databaseContext, IConfiguration configuration)
		{
			_databaseContext = databaseContext;
            this.configuration = configuration;
        }

		// ===============================================================================
		// RESUMES
		// ===============================================================================

		/// <summary>
		/// Add a Resume to personal resumes
		/// </summary>
		[HttpPost]
		[Route("NewPersonalResume")]
		public async Task<ActionResult<ResumeModel>> NewResume(int templateID, string resumeName)
		{
			var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EID == null) return NotFound();
			return await NewResume(templateID, resumeName, EID);
		}

		/// <summary>
		/// Add a Resume to an Employee
		/// </summary>
		[HttpPost]
		[Route("NewResume")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<ResumeModel>> NewResume(int templateID, string resumeName, string EID)
		{

			// Find the template record
			var template = await _databaseContext.Resume_Template.FindAsync(templateID);


			if (template == null)
			{
				return NotFound();
			}

			var employee = await _databaseContext.Employee.FindAsync(EID);

			if (employee == null)
			{
				return NotFound();
			}

			// Find the Sector Types associated with that template
			var sectorTypes = (from t in _databaseContext.Template_Type
							   join s in _databaseContext.SectorType on t.TypeID equals s.TypeID
							   where t.TemplateID == templateID
							   select s)
							  .ToList();


			ResumeEntity entity = new ResumeEntity
			{
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				EID = EID,
				TemplateID = templateID,
				Name = resumeName,
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				Status = Status.Regular.ToString(),
				WID = 0,
				TemplateName = template.Title,
				EmployeeName = employee.Name
			};

			var resume = _databaseContext.Resume.Add(entity);

			foreach (var sector in sectorTypes)
			{
				_databaseContext.Sector.Add(new SectorEntity
				{
					Creation_Date = ControllerHelpers.CurrentTimeAsString(),
					Last_Edited = ControllerHelpers.CurrentTimeAsString(),
					Content = "",
					EID = EID,
					TypeID = sector.TypeID,
					TypeTitle = sector.Title,
					RID = resume.Entity.RID,
					ResumeName = resumeName,
					Division = "",
					Image = ""
				});
			}


			await _databaseContext.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetResume),
				new { RID = resume.Entity.RID },
				 resume.Entity);
		}


		/// <summary>
		/// Delete a Resume
		/// </summary>
		[HttpDelete]
		[Route("DeleteResume")]
		public async Task<IActionResult> DeleteResume(int RID)
		{


			//var resume = Resumes.Find(x => x.RID == RID);

			var resume = await _databaseContext.Resume.FindAsync(RID);

			if (resume == null)
			{
				return NotFound();
			}

			//Resumes.Remove(resume);
			_databaseContext.Resume.Remove(resume);
			await _databaseContext.SaveChangesAsync();
			return Ok();
		}

		/// <summary>
		/// Get a Resume
		/// </summary>
		[HttpGet]
		[Route("GetResume")]
		public async Task<ActionResult<ResumeModel>> GetResume(int RID)
		{

			///var resume = Resumes.Find(x => x.RID == RID);
			var resume = await _databaseContext.Resume.FindAsync(RID);

			if (resume == null)
			{
				return NotFound();
			}

			// Get all sectors for this resume
			ResumeModel result = ControllerHelpers.ResumeEntityToModel(resume);

			var sectors = _databaseContext.Sector.Where(s => s.RID == RID).ToList();

			result.SectorList = new List<SectorModel>();
			foreach (var sector in sectors)
			{
				result.SectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
			}

			return result;
		}

		/// <summary>
		/// Get all Resumes for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetResumesForEmployee")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetResumesForEmployee(string EID)
		{
			//var resumes = Resumes;


			var resumes = _databaseContext.Resume.Where(r => r.EID == EID);
			List<ResumeModel> result = new List<ResumeModel>();
			foreach (var resume in resumes)
			{
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
			}
			return result;
		}

		/// <summary>
		/// Get personal Resumes
		/// </summary>
		[HttpGet]
		[Route("GetPersonalResumes")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetPersonalResumes()
		{
			var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EID == null) return NotFound();
			return await GetPersonalResumesForEmployee(EID);
		}

		/// <summary>
		/// Get personal Resume for an Employee
		/// </summary>
		[HttpGet]
		[Route("GetPersonalResumesForEmployee")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> GetPersonalResumesForEmployee(string EID)
		{


			// TODO: add only three statuses for resumes reqested, regular, exported
			var resumes = _databaseContext.Resume.Where(r => r.EID == EID && 
															(r.Status == Status.Regular.ToString() ||
															 r.Status == Status.Requested.ToString()));

			if (resumes == null)
			{
				return NotFound();
			}

			List<ResumeModel> result = new List<ResumeModel>();
			foreach (var resume in resumes)
			{
				result.Add(ControllerHelpers.ResumeEntityToModel(resume));
			}

			return result;	
		}

		// ===============================================================================
		// SECTORS
		// ===============================================================================

		/// <summary>
		/// Create a new Sector
		/// </summary>
		[HttpPost]
		[Route("NewSector")]
		public async Task<ActionResult<SectorModel>> NewSector(SectorModel model)
		{

			SectorEntity entity = new SectorEntity
			{
				SID = model.SID,
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				Content = model.Content,
				Image = model.Image,
				Division = model.Division
			};

			// Sectors.Add(model);

			_databaseContext.Sector.Add(entity);
			await _databaseContext.SaveChangesAsync();


			return CreatedAtAction(
				nameof(GetSector),
				new { SID = model.SID },
				model);
		}

		/// <summary>
		/// Get a Sector based on SID
		/// </summary>
		[HttpGet]
		[Route("GetSector")]
		public async Task<ActionResult<SectorModel>> GetSector(int SID)
		{


			//var sector = Sectors.Find(x => x.SID == SID);

			var sector = await _databaseContext.Sector.FindAsync(SID);

			if (sector == null)
			{
				return NotFound();
			}
			return ControllerHelpers.SectorEntityToModel(sector);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllSectors")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllSectorsForEmployee()
		{
			var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EID == null) return NotFound();
			return await GetAllSectorsForEmployee(EID);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllSectorsByType")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllSectorsForEmployeeByType(int TypeID)
		{
			var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EID == null) return NotFound();
			return await GetAllSectorsForEmployeeByType(EID, TypeID);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllSectorsForEmployee")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllSectorsForEmployee(string EID)
		{

			var employee = await _databaseContext.Employee.FindAsync(EID);

			if (employee == null)
			{
				return NotFound();
			}

			List<SectorModel> sectorList = new List<SectorModel>();
			var sectors = _databaseContext.Sector.Where(s => s.EID == EID).OrderBy(s => s.TypeID);



			foreach (var sector in sectors)
			{
				sectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
			}


			return sectorList;
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllSectorsForEmployeeByType")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllSectorsForEmployeeByType(string EID, int TypeID)
		{

			var employee = await _databaseContext.Employee.FindAsync(EID);

			if (employee == null)
			{
				return NotFound();
			}

			List<SectorModel> sectorList = new List<SectorModel>();
			var sectors = _databaseContext.Sector.Where(s => s.EID == EID && s.TypeID == TypeID)
				.OrderBy(s => s.TypeID);


			foreach (var sector in sectors)
			{
				sectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
			}


			return sectorList;
		}

		/// <summary>
		/// Delete a Sector from a Resume
		/// </summary>
		[HttpDelete]
		[Route("DeleteSector")]
		public async Task<IActionResult> DeleteSector(int SID)
		{
			//var sector = Sectors.Find(x => x.SID == SID);


			var sector = await _databaseContext.Sector.FindAsync(SID);
			if (sector == null)
			{
				return NotFound();
			}

			//resume.SectorList.Remove(sector);
			_databaseContext.Sector.Remove(sector);
			await _databaseContext.SaveChangesAsync();

			return Ok();
		}

		/// <summary>
		/// Edit a Sector
		/// </summary>
		[HttpPut]
		[Route("EditSector")]
		public async Task<IActionResult> EditSector(int SID, string? content, string? division, string? image)
		{
			//var sector = Sectors.Find(x => x.SID == SID);
			var sector = await _databaseContext.Sector.FindAsync(SID);

			if (sector == null)
			{
				return NotFound();
			}

			// Clean null content
			content = content == null ? "" : content;
			division = division == null ? "" : division;
			image = image == null ? "" : image;

			sector.SID = SID;
			sector.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			sector.Content = content;
			sector.Division = division;
			sector.Image = image;

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(sector);
		}

		/// <summary>
		/// Add a Sector to a Resume
		/// </summary>
		[HttpPost]
		[Route("AddSectorToResume")]
		public async Task<IActionResult> AddSectorToResume(
			int RID, string? content, int typeID, string? division, string? image)
		{

			var resume = await _databaseContext.Resume.FindAsync(RID);


			if (resume == null)
			{
				return NotFound();
			}

			var sectorType = await _databaseContext.SectorType.FindAsync(typeID);
			if (sectorType == null)
			{
				return NotFound();
			}

			// Clean null content
			content = content == null ? "" : content;
			division = division == null ? "" : division;
			image = image == null ? "" : image;


			//resume.SectorList.Add(model);
			SectorEntity sector = new SectorEntity();

			sector.RID = RID;
			sector.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			sector.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			sector.Content = content;
			sector.TypeID = typeID;
			sector.TypeTitle = sectorType.Title;
			sector.EID = resume.EID;
			sector.ResumeName = resume.Name;
			sector.Image = image;
			sector.Division = division;

			_databaseContext.Sector.Add(sector);
			await _databaseContext.SaveChangesAsync();

			return Ok(sector);


		}

		/// <summary>
		/// Edit a Sector on a Resume
		/// </summary>
		[HttpPut]
		[Route("EditResumeSector")]
		public async Task<IActionResult> EditResume(int RID, int SID, SectorModel model)
		{
			var resume = await _databaseContext.Resume.FindAsync(RID);

			if (resume == null)
			{
				return NotFound();
			}

			var sector = await _databaseContext.Sector.FindAsync(SID);

			if (sector == null)
			{
				return NotFound();
			}

			sector.SID = model.SID;
			sector.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			sector.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			sector.Content = model.Content;
			sector.TypeID = model.SectorType;
			sector.RID = RID;

			resume.Last_Edited = ControllerHelpers.CurrentTimeAsString();


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

		// ===============================================================================
		// TEMPLATING
		// ===============================================================================

		[HttpGet]
		[Route("GetAllSectorTypes")]
		public IEnumerable<SectorTypeModel> GetAllSectorTypes()
		{
			var sectorTypes = _databaseContext.SectorType.ToList().
				Select(x => ControllerHelpers.SectorTypeEntityToModel(x));
			return sectorTypes;
		}

		/// <summary>
		/// Get all Resume Templates
		/// </summary>
		[HttpGet]
		[Route("GetAllTemplates")]
		public IEnumerable<TemplateModel> GetAllTemplates()
		{
			var templates = _databaseContext.Resume_Template.ToList();
			List<TemplateModel> result = new List<TemplateModel>();
			foreach (var template in templates)
			{
				result.Add(ControllerHelpers.TemplateEntityToModel(template));
			}

			return result;
		}

		// ===============================================================================
		// EXPORT
		// ===============================================================================

		/// <summary>
		/// Export Resume
		/// </summary>
		[HttpGet]
		[Route("ExportResume")]
		public async Task<IActionResult> ExportResume(int RID)
		{
			var resume = await _databaseContext.Resume.FindAsync(RID);


			if(resume == null)
            {
				return NotFound();
            }


			var sectors = from s in _databaseContext.Sector
						  where s.RID == RID
						  select ControllerHelpers.SectorEntityToModel(s);

			var result = ControllerHelpers.ResumeEntityToModel(resume);

            foreach (var sector in sectors)
            {
				result.SectorList.Add(sector);
            }

			return new JsonResult(result);
		}

		/// <summary>
		/// Export Resumes in Workspace
		/// </summary>
		[HttpGet]
		[Route("ExportResumesInWorkspace")]
		public async Task<IActionResult> ExportResumesInWorkspace(int WID)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if(workspace == null)
            {
				return NotFound();
            }

			var resumes = await (from r in _databaseContext.Resume
						  join s in _databaseContext.Sector on r.RID equals s.RID
						  where r.WID == WID
						  select ControllerHelpers.ResumeEntityToModel(r)).ToListAsync();

			// Get all sectors in resume and set status as exported
			// Employees cannot use exported resumes
			foreach (var resume in resumes)
			{
				resume.SectorList = await  _databaseContext.Sector
					.Where(s => s.RID == resume.RID)
					.Select(s => ControllerHelpers.SectorEntityToModel(s))
					.ToListAsync();
				resume.Status = Status.Exported;
			}
			
			await _databaseContext.SaveChangesAsync();

			return new JsonResult(resumes);
		}

		/// <summary>
		/// Export Resumes in Workspace
		/// </summary>
		[HttpGet]
		[Route("ExportResumesInWorkspaceXML.xml")]
		public async Task<ActionResult<IEnumerable<ResumeModel>>> ExportResumesInWorkspaceXML(int WID)
		{
			var workspace = await _databaseContext.Workspace.FindAsync(WID);

			if (workspace == null)
			{
				return NotFound();
			}

			var resumes = await (from r in _databaseContext.Resume
								 join s in _databaseContext.Sector on r.RID equals s.RID
								 where r.WID == WID
								 select ControllerHelpers.ResumeEntityToModel(r)).ToListAsync();

			// Get all sectors in resume and set status as exported
			// Employees cannot use exported resumes
			foreach (var resume in resumes)
			{
				resume.SectorList = await _databaseContext.Sector
					.Where(s => s.RID == resume.RID)
					.Select(s => ControllerHelpers.SectorEntityToModel(s))
					.ToListAsync();
				resume.Status = Status.Exported;
			}

			await _databaseContext.SaveChangesAsync();			

			return resumes;
		}

		// ===============================================================================
		// SEARCH
		// ===============================================================================

		/// <summary>
		/// Search all Resumes
		/// </summary>
		[HttpGet]
		[Route("SearchResumes")]
		public async Task<IActionResult> SearchResume(string filter)
		{
			return BadRequest("Not implemented");
		}

		/// <summary>
		/// Search all Sectors
		/// </summary>
		[HttpGet]
		[Route("SearchSectors")]
		public async Task<IActionResult> SearchSectors(string filter)
		{
			return BadRequest("Not implemented");
		}


		/// <summary>
		/// Search all Employees
		/// </summary>
		[HttpGet]
		[Route("SearchEmployees")]
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
								  e.EID.ToString().Contains(filter) ||
								  e.JobTitle.Contains(filter)
						  select new EmployeeModel{
							  EID = e.EID,
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
		[Route("SearchAllEmployeeResumes")]
		[Authorize(Policy = "PA")]
		public IEnumerable<ResumeModel> SearchEmployeeResumes(string? filter, string EID)
		{
			// Ensure that null value returns all Employees
			if (filter == null)
			{
				filter = "";
			}

			// Get all Resumes for that EID
			var resumes = from r in _databaseContext.Resume
						  join s in _databaseContext.Sector on r.EID equals s.EID
						  where r.EID == EID && (
						  r.Name.Contains(filter) ||
						  r.Creation_Date.Contains(filter) ||
						  r.Status.Contains(filter) ||
						  r.Last_Edited.Contains(filter) ||
						  r.WID.ToString().Contains(filter) ||
						  r.TemplateID.ToString().Contains(filter) ||
						  s.Content.Contains(filter) ||
						  s.TypeTitle.Contains(filter) ||
						  s.TypeID.ToString().Contains(filter) ||
						  s.Division.Contains(filter) ||
						  s.Image.Contains(filter)
						  )
						  select new ResumeModel {
							  EID = r.EID,
							  CreationDate = DateTime.Parse(r.Creation_Date),
							  LastEditedDate = DateTime.Parse(r.Last_Edited),
							  RID = r.RID,
							  Status = (Status)Enum.Parse(typeof(Status), r.Status),
							  WID = r.WID,
							  Name = r.Name,
							  TemplateID = r.TemplateID,
							  TemplateName = r.TemplateName,
						  };

            foreach (var resume in resumes)
            {
				resume.SectorList = _databaseContext.Sector
					.Where(s => s.RID == resume.RID)
					.Select(s => ControllerHelpers.SectorEntityToModel(s))
					.ToList();

            }

			return resumes;

		}

		/// <summary>
		/// Search all Workspaces
		/// </summary>
		[HttpGet]
		[Route("SearchWorkspaces")]
		[Authorize(Policy = "PA")]
		public async Task<IActionResult> SearchWorkspaces(string filter)
        {
			return BadRequest("Not implemented");
        }

		[HttpGet]
		[Route("SearchEmployeeSectors")]
		[Authorize(Policy = "PA")]
		public IEnumerable<SectorModel> SearchEmployeeSectors(string? filter, string EID)
        {
			// Clean input filter
			filter = filter ?? string.Empty;

			var sectors = (from s in _databaseContext.Sector
						   where s.EID == EID &&
						   (s.Content.Contains(filter) ||
							s.TypeTitle.Contains(filter) ||
							s.Last_Edited.Contains(filter) ||
							s.Creation_Date.Contains(filter) ||
							s.ResumeName.Contains(filter) ||
							s.Division.Contains(filter) ||
							s.Image.Contains(filter))
						   select ControllerHelpers.SectorEntityToModel(s))
						   .ToList();

			return sectors;


        }
	}
}

