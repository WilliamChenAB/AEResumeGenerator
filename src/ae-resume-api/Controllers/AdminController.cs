using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ae_resume_api.Controllers
{
    [Route("Admin")]
    [ApiController]
    public class AdminController : Controller
    {

        readonly DatabaseContext _databaseContext;

        public AdminController(DatabaseContext dbContext)
        {
            _databaseContext = dbContext;            
        }

        /// <summary>
        /// Clean the tables and load in the test data
        /// </summary>
        [HttpPost]
        [Route("LoadTestData")]
        public async Task<IActionResult> LoadTestData()
        {
            // TODO: Implement
            return BadRequest("Not implemented");
        }
        /// <summary>
        ///  Create a new Employee
        /// </summary>
        [HttpPost]
        [Route("NewEmployee")]
        public async Task<ActionResult<EmployeeModel>> NewEmployee([FromBody] EmployeeModel model)
        {

            EmployeeEntity entity = new EmployeeEntity
            {
                EID = model.EID,
                Name = model.Name,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };
            
            _databaseContext.Employee.Add(entity);
             await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployee),
                new { EID = model.EID },
                model);
        }

        

        /// <summary>
        /// Edit an Employee based on thier EID
        /// </summary>
        [HttpPut]
        [Route("EditEmployee")]
        public async Task<IActionResult> EditEmployee(int EID, EmployeeModel employeeModel)
        {
            // Ensure editing the correct Employee
            if (EID != employeeModel.EID)
            {
                return BadRequest();
            }

            // var employee = Employees.Find(x => x.EID == EID);

            
            var employee = await _databaseContext.Employee.FindAsync(EID);

            // Check if the employee already exists
            if (employee == null)
            {
                return NotFound();
            }

            employee.EID = employeeModel.EID;
            employee.Name = employeeModel.Name;
            employee.Email = employeeModel.Email;
            employee.Username = employeeModel.Username;
            employee.Password = employeeModel.Password;
        
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
        public async Task<IActionResult> DeleteEmployee(int EID)
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
        /// Get an Employee from their EID
        /// </summary>
        [HttpGet]
        [Route("GetEmployee")]
        public async Task<ActionResult<EmployeeModel>> GetEmployee(int EID)
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
        public async Task<IActionResult> AssignAccess(int EID, string access)
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

        /// <summary>
        /// Create a new Sector Type
        /// </summary>
        [HttpPost]
        [Route("NewSectorType")]
        public async Task<ActionResult<SectorTypeModel>> NewSectorType(SectorTypeModel model)
        {
            SectorTypeEntity entity = new SectorTypeEntity
            {
                TypeID = model.TypeID,
                Title = model.Title,
                Description = model.Description
            };
            // SectorTypes.Add(model);
            
             _databaseContext.SectorType.Add(entity);
             await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSectorType),
                new { TypeID = model.TypeID },
                model);
        }
        /// <summary>
        /// Edit a Sector Type from its sectorTypeID
        /// </summary>
        [HttpPut]
        [Route("EditSectorType")]
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
            sectorType.TypeID = model.TypeID;
            
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
        public async Task<ActionResult<TemplateModel>> CreateTemplate(TemplateModel model)
        {
            TemplateEntity entity = new TemplateEntity
            {
                TemplateID = model.TemplateID,
                Title = model.Title,
                Description = model.Description,
                Last_Edited = DateTime.Now.ToString("yyyMMdd")
            };

            
            
             _databaseContext.Resume_Template.Add(entity);
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTemplate),
                new { TemplateID = model.TemplateID },
                model);
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
            return ControllerHelpers.TemplateEntityToModel(template);
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
            var sectorTypesIDs =  _databaseContext.Template_Type
                .Where(x => x.TemplateID == template.TemplateID).ToList();

            // Get all Sectors that are in the associative table with matching IDs
            var sectorTypes = _databaseContext.SectorType
                .Where(s => sectorTypesIDs.Any(x => x.TypeID == s.TypeID));

            List<SectorTypeModel> result = new List<SectorTypeModel>();
            foreach (var sectorType in sectorTypes)
            {
                result.Add(ControllerHelpers.SectorTypeEntityToModel(sectorType));
            }

            return result;
        }

        /// <summary>
        /// Edit a Resume Template
        /// </summary>
        [HttpPut]
        [Route("EditTemplate")]
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
            template.TemplateID = model.TemplateID;
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
        public async Task<IActionResult> AssignSectorType(int templateID, int sectorTypeID)
        {
            // var template = templateModels.Find(x => x.TemplateID == templateID);
            
            var template = await _databaseContext.Resume_Template.FindAsync(templateID);


            if (template == null)
            {
                return NotFound();
            }

            //var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            //Create new entry in associative table
            TemplateSectorsEntity entity = new TemplateSectorsEntity
            {
                TemplateID = templateID,
                TypeID = sectorTypeID
            };               
                
            
            _databaseContext.Template_Type.Add(entity);

            await _databaseContext.SaveChangesAsync();
            var templateType = await _databaseContext.Template_Type.FindAsync(entity);
            

            return Ok(templateType);
        }

        /// <summary>
        /// Delete a Resume Template
        [HttpDelete]
        [Route("DeleteTemplate")]
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
        private bool EmployeeExists(long EID)
        {           
            return _databaseContext.Employee.Any(e => e.EID == EID);
        }
        
        

    }
}
