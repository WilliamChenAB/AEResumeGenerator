using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ae_resume_api.Controllers
{

    [Route("Sector")]
    [ApiController]
    public class SectorController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public SectorController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }


		/// <summary>
		/// Create a new Sector
		/// </summary>
		[HttpPost]
		[Route("New")]
		public async Task<ActionResult<SectorModel>> New(SectorModel model)
		{

			SectorEntity entity = new SectorEntity
			{
				Creation_Date = ControllerHelpers.CurrentTimeAsString(),
				Last_Edited = ControllerHelpers.CurrentTimeAsString(),
				Content = model.Content,
				Image = model.Image,
				Division = model.Division,
				ResumeId = model.ResumeId,
				TypeId = model.TypeId
			};

			// Sectors.Add(model);

			_databaseContext.Sector.Add(entity);
			await _databaseContext.SaveChangesAsync();


			return CreatedAtAction(
				nameof(Get),
				new { SectorId = model.SectorId },
				model);
		}


		/// <summary>
		/// Delete a Sector from a Resume
		/// </summary>
		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete(int SectorId)
		{
			var sector = await _databaseContext.Sector.FindAsync(SectorId);
			if (sector == null)
			{
				return NotFound("Sector not found");
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
		[Route("Edit")]
		public async Task<IActionResult> Edit(int SectorId, string? content, string? division, string? image)
		{
			var sector = await _databaseContext.Sector.FindAsync(SectorId);

			if (sector == null)
			{
				return NotFound("Sector not found");
			}

			// Clean null content
			content = content == null ? "" : content;
			division = division == null ? "" : division;
			image = image == null ? "" : image;

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
				return BadRequest(ex.Message);
			}

			return Ok(sector);
		}

		/// <summary>
		/// Get a Sector based on SectorId
		/// </summary>
		[HttpGet]
		[Route("Get")]
		public async Task<ActionResult<SectorModel>> Get(int SectorId)
		{
			var sector = await _databaseContext.Sector.FindAsync(SectorId);

			if (sector == null)
			{
				return NotFound("Sector not found");
			}
			return ControllerHelpers.SectorEntityToModel(sector);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllPersonal")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllPersonal()
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();

			return await GetAllHelper(EmployeeId, null, true);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllPersonalByType")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllPersonalByType(int TypeId)
		{
			var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
			if (EmployeeId == null) return NotFound();

			return await GetAllHelper(EmployeeId, TypeId, true);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllForEmployee")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllForEmployee(string EmployeeId)
		{
			return await GetAllHelper(EmployeeId, null, false);
		}

		/// <summary>
		/// Get all Sectors from an Employee
		/// </summary>
		[HttpGet]
		[Route("GetAllForEmployeeByType")]
		[Authorize(Policy = "PA")]
		public async Task<ActionResult<IEnumerable<SectorModel>>> GetAllForEmployeeByType(string EmployeeId, int TypeId)
		{
			return await GetAllHelper(EmployeeId, TypeId, false);
		}

		private async Task<ActionResult<IEnumerable<SectorModel>>> GetAllHelper(string EmployeeId, int? TypeId, bool personalOnly)
		{
			var guid = Guid.Parse(EmployeeId);
			var employee = await _databaseContext.Employee.FindAsync(guid);
			if (employee == null) return NotFound("Employee not found");

			List<SectorModel> sectorList = new List<SectorModel>();
			var sectors =
				_databaseContext.Sector.Where(s => s.Resume.EmployeeId == guid)
				.ToList()
				.Where(
					s => (TypeId == null ? true : s.TypeId == TypeId) &&
					(!personalOnly || ControllerHelpers.ResumeIsPersonal(s.Resume)))
				.OrderBy(s => s.TypeId);

			foreach (var sector in sectors)
			{
				sectorList.Add(ControllerHelpers.SectorEntityToModel(sector));
			}

			return sectorList;
		}


		/// <summary>
		/// Add a Sector to a Resume
		/// </summary>
		[HttpPost]
		[Route("AddToResume")]
		public async Task<IActionResult> AddSectorToResume(int ResumeId, string? content, int TypeId, string? division, string? image)
		{
			var resume = await _databaseContext.Resume.FindAsync(ResumeId);

			if (resume == null)
			{
				return NotFound("Resume not found");
			}

			// Clean null content
			content = content == null ? "" : content;
			division = division == null ? "" : division;
			image = image == null ? "" : image;

			//resume.SectorList.Add(model);
			SectorEntity sector = new SectorEntity();

			sector.ResumeId = ResumeId;
			sector.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			sector.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			sector.Content = content;
			sector.TypeId = TypeId;
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
		public async Task<IActionResult> EditResumeSector(int SectorId, SectorModel model)
		{
			var sector = await _databaseContext.Sector.FindAsync(SectorId);

			if (sector == null)
			{
				return NotFound("Sector not found");
			}

			sector.SectorId = model.SectorId;
			sector.Creation_Date = ControllerHelpers.CurrentTimeAsString();
			sector.Last_Edited = ControllerHelpers.CurrentTimeAsString();
			sector.Content = model.Content;
			sector.TypeId = model.TypeId;

			sector.Resume.Last_Edited = ControllerHelpers.CurrentTimeAsString();

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok(ControllerHelpers.ResumeEntityToModel(sector.Resume));

		}

	}

}
