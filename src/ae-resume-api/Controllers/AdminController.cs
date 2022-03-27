using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ae_resume_api.Controllers
{
    [Route("Admin")]
    [ApiController]
    public class AdminController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public AdminController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// Clean the tables and load in the test data
        /// </summary>
        [HttpPost]
        [Route("LoadTestData")]
        [Authorize (Policy = "SA")]
        public async Task<IActionResult> LoadTestData()
        {
            // TODO: Implement
            return BadRequest("Not implemented");
        }

        // ===============================================================================
        // EMPLOYEES
        // ===============================================================================

        /// <summary>
        /// Edit an Employee
        /// </summary>
        [HttpPut]
        [Route("EditEmployee")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> EditEmployee(EmployeeModel employeeModel)
        {

            var employee = await _databaseContext.Employee.FindAsync(employeeModel.EID);

            // Check if the employee already exists
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            employee.Name = employeeModel.Name;
            employee.Email = employeeModel.Email;
            employee.JobTitle = employeeModel.JobTitle;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

            return Ok(employee);
        }

        /// <summary>
        /// Delete an Employee
        /// </summary>
        [HttpDelete]
        [Route("DeleteEmployee")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> DeleteEmployee(string EID)
        {


            var employee = await _databaseContext.Employee.FindAsync(EID);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _databaseContext.Employee.Remove(employee);
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
        /// Get self
        /// </summary>
        [HttpGet]
        [Route("GetOwnEmployee")]
        public async Task<ActionResult<EmployeeModel>> GetOwnEmployee()
        {
            var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
            if (EID == null) return NotFound();
            return await GetEmployee(EID);
        }

        /// <summary>
        /// Get an Employee from their EID
        /// </summary>
        [HttpGet]
        [Route("GetEmployee")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<EmployeeModel>> GetEmployee(string EID)
        {            
            var employee = await _databaseContext.Employee.FindAsync(EID);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            return ControllerHelpers.EmployeeEntityToModel(employee);
        }

        /// <summary>
        /// Get all Employees
        /// </summary>
        [HttpGet]
        [Route("GetAllEmployees")]
        public IEnumerable<EmployeeModel> GetAllEmployees()
        {
            var employees = _databaseContext.Employee.ToList();
            List<EmployeeModel> result = new List<EmployeeModel>();
            foreach (var employee in employees)
            {
                result.Add(ControllerHelpers.EmployeeEntityToModel(employee));
            }

            return result;
        }

        /// <summary>
        /// Assign of change an Employees acccess
        /// </summary>
        [HttpPost]
        [Route("AssignAccess")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> AssignAccess(string EID, Access access)
        {
            var employee = await _databaseContext.Employee.FindAsync(EID);

            // Check if the employee already exists
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            employee.Access = access;


            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

            return Ok(employee);

        }

        // ===============================================================================
        // TEMPLATING
        // ===============================================================================
        
        /// <summary>
        /// Create a new Sector Type
        /// </summary>
        [HttpPost]
        [Route("NewSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<SectorTypeModel>> NewSectorType(string title, string description)
        {
            var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
            if (EID == null) return NotFound();

            SectorTypeEntity entity = new SectorTypeEntity
            {
                Title = title,
                Description = description,
                EID = EID
            };
            // SectorTypes.Add(model);

             var result = _databaseContext.SectorType.Add(entity);
             await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSectorType),
                new { TypeID = result.Entity.TypeID },
                result.Entity);
        }
        /// <summary>
        /// Edit a Sector Type from its sectorTypeID
        /// </summary>
        [HttpPut]
        [Route("EditSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> EditSectorType(int sectorTypeID, string title, string description)
        {

            var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

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
        [Route("EditSectorTypeTitle")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> EditSectorTypeTitle(int sectorTypeID, string title)
        {
            var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if(sectorType == null)
            {
                return NotFound("Sector Type not found");
            }

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
        /// Get Sector Type from its sectorTypeID
        /// </summary>
        [HttpGet]
        [Route("GetSectorType")]
        public async Task<ActionResult<SectorTypeModel>> GetSectorType(int sectorTypeID)
        {
            //var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);

            var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if (sectorType == null)
            {
                return NotFound("Sector Type not found");
            }
            return ControllerHelpers.SectorTypeEntityToModel(sectorType);
        }


        /// <summary>
        /// Delete a Sector Type
        /// </summary>
        [HttpDelete]
        [Route("DeleteSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> DeleteSectorType(int sectorTypeID)
        {
            // var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);

            var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if (sectorType == null)
            {
                return NotFound("Sector Type not found");
            }

            // SectorTypes.Remove(sectorType);

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

        /// <summary>
        /// Create a new Resume Template
        /// </summary>
        [HttpPost]
        [Route("CreateTemplate")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<TemplateModel>> CreateTemplate([FromBody] TemplateModel model)
        {
            TemplateEntity entity = new TemplateEntity
            {
                Title = model.Title,
                Description = model.Description,
                Last_Edited = DateTime.Now.ToString("yyyyMMdd HH:mm:ss")
            };

            // Add the new Template and get its ID to add types to associative table
            var result = _databaseContext.Resume_Template.Add(entity).Entity;
            await _databaseContext.SaveChangesAsync();            

            // Add Sector Types to DB
            foreach (var sectorType in model.SectorTypes)
            {
                _databaseContext.Template_Type.Add(new TemplateSectorsEntity
                {
                    TemplateID = result.TemplateID,
                    TypeID = sectorType.TypeID
                });
            }
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTemplate),
                new { TemplateID = model.TemplateID },
                result);
        }

        /// <summary>
        /// Get Resume Template
        /// </summary>
        [HttpGet]
        [Route("GetTemplate")]
        public async Task<ActionResult<TemplateModel>> GetTemplate(int templateID)
        {
            //var template = templateModels.Find(x => x.TemplateID == templateID);

            var template = await _databaseContext.Resume_Template.FindAsync(templateID);

            if (template == null)
            {
                return NotFound("Template not found");
            }

            // Convert template to Model and add sector types
            var result = ControllerHelpers.TemplateEntityToModel(template);
            //result.SectorTypes = (from s in _databaseContext.Template_Type
            //                     join t in _databaseContext.SectorType on s.TypeID equals t.TypeID
            //                     where s.TemplateID == templateID
            //                     select ControllerHelpers.SectorTypeEntityToModel(t)).ToList();

            return result;
        }

        /// <summary>
        /// Get all the SectorTypes in a Resume Template
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSectorsInTemplate")]
        public async Task<ActionResult<IEnumerable<SectorTypeModel>>> GetSectorsInTemplate(int templateID)
        {

            //var template = templateModels.Find(x => x.TemplateID == templateID);            
            var template = await _databaseContext.Resume_Template.FindAsync(templateID);

            if (template == null)
            {
                return new List<SectorTypeModel>();
            }


            // Get the Sector Type IDs from associative table
            // Get all Sectors that are in the associative table with matching IDs
            var sectorTypesModel = (from t in _databaseContext.Template_Type
                                    join s in _databaseContext.SectorType on t.TypeID equals s.TypeID
                                    where t.TemplateID == template.TemplateID
                                    select ControllerHelpers.SectorTypeEntityToModel(s))
                                   .ToList();


            return sectorTypesModel;
        }

        /// <summary>
        /// Edit a Resume Template
        /// </summary>
        [HttpPut]
        [Route("EditTemplate")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> EditTemplate(int templateID, string title, string description)
        {

            var template = await _databaseContext.Resume_Template.FindAsync(templateID);

            if (template == null)
            {
                return NotFound("Template not found");
            }

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
        /// Assign a Sector Type to a Resume Template
        /// </summary>
        [HttpPost]
        [Route("AssignSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> AssignSectorType(int templateID, IEnumerable<int> sectorTypeID)
        {
            // var template = templateModels.Find(x => x.TemplateID == templateID);
            var template = await _databaseContext.Resume_Template.FindAsync(templateID);


            if (template == null)
            {
                return NotFound("Template does not exist");
            }



            //Re wite associative table with new values
            var existingTypes = await _databaseContext.Template_Type.Where(x => x.TemplateID == templateID)
                .ToListAsync();
            existingTypes.ForEach(x => _databaseContext.Template_Type.Remove(x));

            foreach (var id in sectorTypeID)
            {
                TemplateSectorsEntity entity = new TemplateSectorsEntity
                {
                    TemplateID = templateID,
                    TypeID = id
                };
                _databaseContext.Template_Type.Add(entity);
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
        /// Delete a Resume Template
        [HttpDelete]
        [Route("DeleteTemplate")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> DeleteTemplate(int templateID)
        {

            var template = await _databaseContext.Resume_Template.FindAsync(templateID);

            if (template == null)
            {
                return NotFound("Template not found");
            }            

            _databaseContext.Resume_Template.Remove(template);
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
        /// Check to see if an Employee Exists in the db
        /// </summary>
        private bool EmployeeExists(string EID)
        {
            return _databaseContext.Employee.Any(e => e.EID == EID);
        }



    }
}
