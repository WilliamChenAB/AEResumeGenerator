using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ae_resume_api.Controllers
{

    [Route("Template")]
    [ApiController]
    public class TemplateController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public TemplateController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }


        /// <summary>
        /// Create a new Sector Type
        /// </summary>
        /// <summary>
        /// Create a new Resume Template
        /// </summary>
        [HttpPost]
        [Route("Create")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<TemplateModel>> Create([FromBody] TemplateModel model)
        {
            TemplateEntity entity = new TemplateEntity
            {
                Title = model.Title,
                Description = model.Description,
                Last_Edited = DateTime.Now.ToString("yyyyMMdd HH:mm:ss")
            };

            // Add the new Template and get its ID to add types to associative table
            var result = _databaseContext.Template.Add(entity).Entity;
            await _databaseContext.SaveChangesAsync();

            // Add Sector Types to DB
            foreach (var sectorType in model.SectorTypes)
            {
                _databaseContext.TemplateSector.Add(new TemplateSectorEntity
                {
                    TemplateId = result.TemplateId,
                    TypeId = sectorType.TypeId
                });
            }
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { TemplateID = model.TemplateId },
                result);
        }

        /// <summary>
        /// Get Resume Template
        /// </summary>
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<TemplateModel>> Get(int templateID)
        {
            var template = await _databaseContext.Template.FindAsync(templateID);
            if (template == null) return NotFound("Template not found");

            // Convert template to Model and add sector types
            var result = ControllerHelpers.TemplateEntityToModel(template);
            //result.SectorTypes = (from s in _databaseContext.Template_Type
            //                     join t in _databaseContext.SectorType on s.TypeId equals t.TypeId
            //                     where s.TemplateID == templateID
            //                     select ControllerHelpers.SectorTypeEntityToModel(t)).ToList();

            return result;
        }


        /// <summary>
        /// Edit a Resume Template
        /// </summary>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> Edit(int templateID, string? title, string? description)
        {
            var template = await _databaseContext.Template.FindAsync(templateID);
            if (template == null) return NotFound("Template not found");

            // Clean input filter
            title = title ?? "";
            description = description ?? "";

            template.Title = title;
            template.Description = description;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(template);
        }

        /// <summary>
        /// Delete a Resume Template
        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> Delete(int templateID)
        {
            var template = await _databaseContext.Template.FindAsync(templateID);
            if (template == null) return NotFound("Template not found");

            _databaseContext.Template.Remove(template);

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

        /// <summary>
        /// Get all the SectorTypes in a Resume Template
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSectors")]
        public async Task<ActionResult<IEnumerable<SectorTypeModel>>> GetSectors(int templateID)
        {
            var template = await _databaseContext.Template.FindAsync(templateID);

            if (template == null) return new List<SectorTypeModel>();

            return template.TemplateSectors
                .Select(s => ControllerHelpers.SectorTypeEntityToModel(s.SectorType))
                .ToList();
        }

        /// <summary>
        /// Assign a Sector Type to a Resume Template
        /// </summary>
        [HttpPost]
        [Route("AssignSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> AssignSectorType(int templateID, IEnumerable<int> SectorTypeId)
        {
            var template = await _databaseContext.Template.FindAsync(templateID);
            if (template == null) return NotFound("Template does not exist");

            //Re wite associative table with new values
            template.TemplateSectors.ForEach(x => _databaseContext.TemplateSector.Remove(x));

            foreach (var id in SectorTypeId)
            {
                TemplateSectorEntity entity = new TemplateSectorEntity
                {
                    TemplateId = templateID,
                    TypeId = id
                };
                _databaseContext.TemplateSector.Add(entity);
            }

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(template);
        }

        /// <summary>
        /// Get all Resume Templates
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<TemplateModel> GetAll()
        {
            var templates = _databaseContext.Template.ToList();
            List<TemplateModel> result = new List<TemplateModel>();
            foreach (var template in templates)
            {
                result.Add(ControllerHelpers.TemplateEntityToModel(template));
            }

            return result;
        }


    }
}
