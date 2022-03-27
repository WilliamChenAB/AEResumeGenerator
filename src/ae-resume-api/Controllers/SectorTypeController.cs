using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ae_resume_api.Controllers
{

    [Route("SectorType")]
    [ApiController]
    public class SectorTypeController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public SectorTypeController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("New")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<SectorTypeModel>> New(string title, string description)
        {
            SectorTypeEntity entity = new SectorTypeEntity
            {
                Title = title,
                Description = description
            };
            // SectorTypes.Add(model);

            var result = _databaseContext.SectorType.Add(entity);
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { TypeId = result.Entity.TypeId },
                result.Entity);
        }


        /// <summary>
        /// Edit a Sector Type from its SectorTypeId
        /// </summary>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> Edit(int SectorTypeId, string title, string description)
        {
            var sectorType = await _databaseContext.SectorType.FindAsync(SectorTypeId);

            if (sectorType == null)
            {
                return NotFound("Sector Type not found");
            }

            sectorType.Title = title;
            sectorType.Description = description;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

            return Ok(sectorType);

        }

        [HttpPut]
        [Route("EditTitle")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> EditTitle(int SectorTypeId, string title)
        {
            var sectorType = await _databaseContext.SectorType.FindAsync(SectorTypeId);

            if (sectorType == null) return NotFound("Sector Type not found");
            sectorType.Title = title;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(sectorType);

        }

        /// <summary>
        /// Get Sector Type from its SectorTypeId
        /// </summary>
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<SectorTypeModel>> Get(int SectorTypeId)
        {
            var sectorType = await _databaseContext.SectorType.FindAsync(SectorTypeId);

            if (sectorType == null) return NotFound("Sector Type not found");
            return ControllerHelpers.SectorTypeEntityToModel(sectorType);
        }


        /// <summary>
        /// Delete a Sector Type
        /// </summary>
        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> Delete(int SectorTypeId)
        {
            var sectorType = await _databaseContext.SectorType.FindAsync(SectorTypeId);

            if (sectorType == null) return NotFound("Sector Type not found");

            _databaseContext.SectorType.Remove(sectorType);

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<SectorTypeModel> GetAll()
        {
            var sectorTypes =
                _databaseContext.SectorType
                .ToList()
                .Select(x => ControllerHelpers.SectorTypeEntityToModel(x));
            return sectorTypes;
        }

    }

}
