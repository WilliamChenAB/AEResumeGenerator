using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
                return NotFound();
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
                return NotFound(ex.Message);

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

            // var employee = Employees.Find(x => x.EID == EID);

            var employee = await _databaseContext.Employee.FindAsync(EID);

            if (employee == null)
            {
                return NotFound();
            }

            // Employees.Remove(employee);

            _databaseContext.Employee.Remove(employee);
            await _databaseContext.SaveChangesAsync();
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
            // var employee = Employees.Find(x => x.EID == EID);

            var employee = await _databaseContext.Employee.FindAsync(EID);

            if (employee == null)
            {
                return NotFound();
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
            // var employee = Employees.Find(x => x.EID == EID);


            var employee = await _databaseContext.Employee.FindAsync(EID);

            // Check if the employee already exists
            if (employee == null)
            {
                return NotFound();
            }

            employee.Access = access;


            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }

            return Ok(employee);

        }

        // ===============================================================================
        // TEMPLATING
        // ===============================================================================

        // TODO: Fix this so it doesn't take a model with unneeded params
        /// <summary>
        /// Create a new Sector Type
        /// </summary>
        [HttpPost]
        [Route("NewSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<SectorTypeModel>> NewSectorType(SectorTypeModel model)
        {
            var EID = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
            if (EID == null) return NotFound();

            SectorTypeEntity entity = new SectorTypeEntity
            {
                Title = model.Title,
                Description = model.Description,
                EID = EID
            };
            // SectorTypes.Add(model);

             var result = _databaseContext.SectorType.Add(entity);
             await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSectorType),
                new { TypeID = result.Entity.TypeID },
                model);
        }
        /// <summary>
        /// Edit a Sector Type from its sectorTypeID
        /// </summary>
        [HttpPut]
        [Route("EditSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> EditSectorType(int sectorTypeID, SectorTypeModel model)
        {
            if (sectorTypeID != model.TypeID)
            {
                return BadRequest();
            }
            // var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);

            var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if (sectorType == null)
            {
                return NotFound();
            }

            sectorType.Title = model.Title;
            sectorType.Description = model.Description;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

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
                return NotFound();
            }

            sectorType.Title = title;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

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
                return NotFound();
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
                return NotFound();
            }

            // SectorTypes.Remove(sectorType);

            _databaseContext.SectorType.Remove(sectorType);
            await _databaseContext.SaveChangesAsync();
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
            _databaseContext.Resume_Template.Add(entity);
            await _databaseContext.SaveChangesAsync();
            var template = _databaseContext.Resume_Template.OrderBy(x => x.TemplateID).Last();

            // Add Sector Types to DB
            foreach (var sectorType in model.SectorTypes)
            {
                _databaseContext.Template_Type.Add(new TemplateSectorsEntity
                {
                    TemplateID = template.TemplateID,
                    TypeID = sectorType.TypeID
                });
            }
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTemplate),
                new { TemplateID = model.TemplateID },
                entity);
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
                return NotFound();
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
                return NotFound();
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
        public async Task<IActionResult> EditTemplate(int templateID, TemplateModel model)
        {

            if (templateID != model.TemplateID)
            {
                return BadRequest();
            }
            // var template = templateModels.Find(x => x.TemplateID == templateID);

            var template = await _databaseContext.Resume_Template.FindAsync(templateID);

            if (template == null)
            {
                return NotFound();
            }

            template.Title = model.Title;
            template.Description = model.Description;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }

            return Ok(template);
        }

        /// <summary>
        /// Assign a Sector Type to a Resume Template
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="sectorTypeID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AssignSectorType")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> AssignSectorType(int templateID, IEnumerable<int> sectorTypeID)
        {
            // var template = templateModels.Find(x => x.TemplateID == templateID);
            var template = await _databaseContext.Resume_Template.FindAsync(templateID);


            if (template == null)
            {
                return NotFound();
            }

            //var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            //Create new entry in associative table
            foreach (var id in sectorTypeID)
            {
                TemplateSectorsEntity entity = new TemplateSectorsEntity
                {
                    TemplateID = templateID,
                    TypeID = id
                };


                _databaseContext.Template_Type.Add(entity);
            }

            await _databaseContext.SaveChangesAsync();


            return Ok(template);
        }

        /// <summary>
        /// Delete a Resume Template
        [HttpDelete]
        [Route("DeleteTemplate")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> DeleteTemplate(int templateID)
        {


            //var template = templateModels.Find(x => x.TemplateID == templateID);

            var template = await _databaseContext.Resume_Template.FindAsync(templateID);

            if (template == null)
            {
                return NotFound();
            }

            //templateModels.Remove(template);

            _databaseContext.Resume_Template.Remove(template);
             await _databaseContext.SaveChangesAsync();
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
