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
                Last_Edited = ControllerHelpers.CurrentTimeAsString()
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
            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            

            return CreatedAtAction(
                nameof(Get),
                new { TemplateId = model.TemplateId },
                result);
        }

        /// <summary>
        /// Get Resume Template
        /// </summary>
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<TemplateModel>> Get(int TemplateId)
        {
            var template = await _databaseContext.Template.FindAsync(TemplateId);
            if (template == null) return NotFound("Template not found");

            // Convert template to Model and add sector types
            var result = ControllerHelpers.TemplateEntityToModel(template);
            //result.SectorTypes = (from s in _databaseContext.Template_Type
            //                     join t in _databaseContext.SectorType on s.TypeId equals t.TypeId
            //                     where s.TemplateId == TemplateId
            //                     select ControllerHelpers.SectorTypeEntityToModel(t)).ToList();

            return result;
        }


        /// <summary>
        /// Edit a Resume Template
        /// </summary>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> Edit(int TemplateId, string? title, string? description)
        {
            var template = await _databaseContext.Template.FindAsync(TemplateId);
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
        public async Task<IActionResult> Delete(int TemplateId)
        {
            var template = await _databaseContext.Template.FindAsync(TemplateId);
            if (template == null) return NotFound("Template not found");

            // Don't delete this - it loads the resumes into memory so that ClientCascade works correctly.
            // We're not using regular Cascading because it causes deletion cycles, so this lets us control the scope of the cascade
            // If you're curious, see https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete?msclkid=cd55484aaf3e11ecbedf860b25799ab7#database-cascade-limitations
            var resumes = template.Resumes;

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
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSectors")]
        public async Task<ActionResult<IEnumerable<SectorTypeModel>>> GetSectors(int TemplateId)
        {
            var template = await _databaseContext.Template.FindAsync(TemplateId);

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
        public async Task<IActionResult> AssignSectorType(int TemplateId, IEnumerable<int> sectorTypeId)
        {
            var template = await _databaseContext.Template.FindAsync(TemplateId);
            if (template == null) return NotFound("Template does not exist");

            //Re wite associative table with new values
            template.TemplateSectors.ForEach(x => _databaseContext.TemplateSector.Remove(x));

            foreach (var id in sectorTypeId)
            {
                TemplateSectorEntity entity = new TemplateSectorEntity
                {
                    TemplateId = TemplateId,
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
