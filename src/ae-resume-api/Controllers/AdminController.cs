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
        public List<EmployeeModel> Employees = new List<EmployeeModel>();
        public List<SectorTypeModel> SectorTypes = new List<SectorTypeModel>();
        public List<TemplateModel> templateModels = new List<TemplateModel>();

        public AdminController(DatabaseContext dbContext)
        {
            _databaseContext = dbContext;


            // _adminservice = adminservice;

            // Add Mock data for front end testing
            // REMOVE FOR FINAL
            this.Employees = new List<EmployeeModel>();
            Employees.Add(new EmployeeModel
            {
                EID = 5,
                Name = "James",
                Email = "email",
                Username = "James",
                Password = "password"
            });


            SectorTypes.Add(new SectorTypeModel
            {
                TypeID = 5,
                Description = "Test type",
                Title = "Test"
            });

            templateModels.Add(new TemplateModel
            {
                TemplateID = 5,
                Description = "Test template",
                Title = "Test",
                SectorTypes = new List<SectorTypeModel> {
                    new SectorTypeModel{
                        Title = "test Type",
                        Description = "Description",
                        TypeID = 2}
                }
            });
        }

        /// <summary>
        ///  Create a new Employee
        /// </summary>
        [HttpPost]
        [Route("NewEmployee")]
        public async Task<ActionResult<EmployeeModel>> NewEmployee([FromBody] EmployeeModel model)
        {
            // Temp list for testing
            Employees.Add(model);


            EmployeeEntity entity = new EmployeeEntity
            {
                EID = model.EID,
                Name = model.Name,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };
            // TODO: Implement DB connection
            //_databaseContext.Employees.Add(entity);
            // await _databaseContext.SaveChangesAsync();

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

            var employee = Employees.Find(x => x.EID == EID);

            // TODO: Implement DB connection
            //var employee = await _databaseContext.Employees.FindAsync(EID);

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

            // TODO: Implement DB connection
            //try
            //{
            //     await _databaseContext.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    return NotFound(ex.Message);

            //}

            return Ok(employee);
        }

        /// <summary>
        /// Delete an Employee
        /// </summary>
        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int EID)
        {

            var employee = Employees.Find(x => x.EID == EID);
            // TODO: Implement DB connection
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (employee == null)
            {
                return NotFound();
            }

            Employees.Remove(employee);
            // TODO: Implement DB connection
            // _databaseContext.Employees.Remove(employee);
            // await _databaseContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Get an Employee from their EID
        /// </summary>
        [HttpGet]
        [Route("GetEmployee")]
        public async Task<ActionResult<EmployeeModel>> GetEmployee(int EID)
        {
            var employee = Employees.Find(x => x.EID == EID);
            // TODO: Implement DB connection
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (employee == null)
            {
                return NotFound();
            }
            return employee;
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
                result.Add(EmployeeEntityToModel(employee));
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
            var employee = Employees.Find(x => x.EID == EID);

            // TODO: Implement DB connection
            //var employee = await _databaseContext.Employee.FindAsync(EID);

            // Check if the employee already exists
            if (employee == null)
            {
                return NotFound();
            }

            employee.Access = access;

            // TODO: Implement DB connection
            //try
            //{
            //     await _databaseContext.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    return NotFound(ex.Message);

            //}

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
            SectorTypes.Add(model);
            // TODO: Implement DB connection
            // _databaseContext.SectorType.Add(entity);
            // await _databaseContext.SaveChangesAsync();

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
            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            // TODO: Implement DB connection
            //var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if (sectorType == null)
            {
                return NotFound();
            }

            sectorType.Title = model.Title;
            sectorType.Description = model.Description;
            sectorType.TypeID = model.TypeID;

            // TODO: Implement DB connection
            //try
            //{
            //     await _databaseContext.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    return NotFound(ex.Message);

            //}

            return Ok(sectorType);

        }

        /// <summary>
        /// Get Sector Type from its sectorTypeID
        /// </summary>
        [HttpGet]
        [Route("GetSectorType")]
        public async Task<ActionResult<SectorTypeModel>> GetSectorType(int sectorTypeID)
        {
            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            // TODO: Implement DB connection
            // var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if (sectorType == null)
            {
                return NotFound();
            }
            return sectorType;
        }

        /// <summary>
        /// Delete a Sector Type
        /// </summary>
        [HttpDelete]
        [Route("DeleteSectorType")]
        public async Task<IActionResult> DeleteSectorType(int sectorTypeID)
        {
            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            // TODO: Implement DB connection
            //var sectorType = await _databaseContext.SectorType.FindAsync(sectorTypeID);

            if (sectorType == null)
            {
                return NotFound();
            }

            SectorTypes.Remove(sectorType);
            // TODO: Implement DB connection
            // _databaseContext.SectorType.Remove(sectorType);
            // await _databaseContext.SaveChangesAsync();
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
                Description = model.Description
            };

            templateModels.Add(model);
            // TODO: Implement DB connection
            // _databaseContext.Template.Add(entity);
            // await _databaseContext.SaveChangesAsync();

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
            var template = templateModels.Find(x => x.TemplateID == templateID);
            // TODO: Implement DB connection
            //var template = await _databaseContext.templateModel.FindAsync(templateID);

            if (template == null)
            {
                return NotFound();
            }
            return template;
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

            var template = templateModels.Find(x => x.TemplateID == templateID);
            // TODO: Implement DB connection
            //var template = await _databaseContext.templateModel.FindAsync(templateID);

            if (template == null)
            {
                return NotFound();
            }
            return template.SectorTypes;
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
            var template = templateModels.Find(x => x.TemplateID == templateID);
            // TODO: Implement DB connection
            //var template = await _databaseContext.templateModel.FindAsync(templateID);

            if (template == null)
            {
                return NotFound();
            }

            template.Title = model.Title;
            template.TemplateID = model.TemplateID;
            template.Description = model.Description;
            template.SectorTypes = model.SectorTypes;

            // TODO: Implement DB connection
            //try
            //{
            //     await _databaseContext.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    return NotFound(ex.Message);

            //}

            return Ok(template);
        }

        /// <summary>
        /// Assign a Sector Type to a Resume Template
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="sectorTypeID"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("AssignSectorType")]
        public async Task<IActionResult> AssignSectorType(int templateID, int sectorTypeID)
        {
            var template = templateModels.Find(x => x.TemplateID == templateID);
            // TODO: Implement DB connection
            //var template = await _databaseContext.templateModels.FindAsync(templateID);


            if (template == null)
            {
                return NotFound();
            }

            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            // TODO: Implement DB connection
            //var template = await _databaseContext.templateModels.FindAsync(templateID);

            template.SectorTypes.Add(sectorType);

            // TODO: Implement DB connection
            //try
            //{
            //     await _databaseContext.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    return NotFound(ex.Message);

            //}

            return Ok(template);
        }

        /// <summary>
        /// Delete a Resume Template
        [HttpDelete]
        [Route("DeleteTemplate")]
        public async Task<IActionResult> DeleteTemplate(int templateID)
        {


            var template = templateModels.Find(x => x.TemplateID == templateID);
            // TODO: Implement DB connection
            //var template = await _databaseContext.templateModels.FindAsync(templateID);

            if (template == null)
            {
                return NotFound();
            }

            templateModels.Remove(template);
            // TODO: Implement DB connection
            //_databaseContext.templateModels.Remove(employee);
            // await _databaseContext.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Check to see if an Employee Exists in the db
        /// </summary>
        private bool EmployeeExists(long EID)
        {
            return Employees.Any(e => e.EID == EID);
            // TODO: Implement DB connection
            // return _databaseContext.Employees.Any(e => e.EID == EID);
        }
        /// <summary>
        /// Translate the Employee entity to model used
        /// </summary>
        public static EmployeeModel EmployeeEntityToModel(EmployeeEntity entity) =>
            new EmployeeModel
            {
                EID = entity.EID,
                Email = entity.Email,
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
            };
    }
}
