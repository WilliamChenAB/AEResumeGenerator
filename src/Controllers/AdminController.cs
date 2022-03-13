using ae_resume_api.Authentication;
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

        private readonly IAdminService _adminservice;

        readonly DatabaseContext _databaseContext;
        public List<EmployeeModel> Employees = new List<EmployeeModel>();
        public List<SectorTypeModel> SectorTypes = new List<SectorTypeModel>();
        public List<TemplateModel> templateModels = new List<TemplateModel>();

        public AdminController (DatabaseContext dbContext)
        {

            _databaseContext = dbContext;

            // _adminservice = adminservice;
            this.Employees = new List<EmployeeModel>();
            Employees.Add(new EmployeeModel { 
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

            templateModels.Add(new TemplateModel { 
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

        [HttpPost]
        [Route("NewEmployee")]
        public async Task<ActionResult<EmployeeModel>> NewEmployee([FromBody] EmployeeModel model)
        {
            // return await _adminservice.CreateEmployee(model);

            Employees.Add(model);
            EmployeeEntity entity = new EmployeeEntity
            {
                EID = model.EID,
                Name = model.Name,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };
            //_databaseContext.Employees.Add(entity);

            return CreatedAtAction(
                nameof(GetEmployee),
                new { EID = model.EID },
                model);
        }

        [HttpPut]
        [Route("EditEmployee")]
        public async Task<IActionResult> EditEmployee(int EID, EmployeeModel employeeModel)
        {
            //return await _adminservice.EditEmployee(Eid, employeeModel);

            if(EID != employeeModel.EID)
            {
                return BadRequest();
            }
            var employee = Employees.Find(x => x.EID == EID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (employee == null)
            {
                return NotFound();
            }

            employee.EID= employeeModel.EID;
            employee.Name= employeeModel.Name;
            employee.Email= employeeModel.Email;
            employee.Username= employeeModel.Username;
            employee.Password= employeeModel.Password;

            return Ok(employee);


        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int EID)
        {
            //return await _adminservice.DeleteEmployee(EID);

            var employee = Employees.Find(x => x.EID == EID);

            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (employee == null)
            {
                return NotFound();
            }
            
            Employees.Remove(employee);
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok();
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<ActionResult<EmployeeModel>> GetEmployee(int EID)
        {
            //return await _adminservice.GetEmployee(EID);

            var employee = Employees.Find(x => x.EID == EID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(employee == null)
            {
                return NotFound();
            }
            return employee;

        }

        [HttpPost]
        [Route("AssignAccess")]
        public async Task<HttpResponseMessage> AssignAccess(int EID, string access)
        {
            return await _adminservice.AssignEmployeeAccess(EID, access);
        }

        [HttpPost]
        [Route("NewSectorType")]
        public async Task<IActionResult> NewSectorType(SectorTypeModel model)
        {
            //return await _adminservice.NewSectorType(model);

             SectorTypes.Add(model);

            SectorTypeEntity entity = new SectorTypeEntity
            {
                TypeID = model.TypeID,
                Title = model.Title,
                Description = model.Description
            };
            //_databaseContext.Employees.Add(entity);

            return CreatedAtAction(
                nameof (GetSectorType),
                new { TypeID = model.TypeID },
                model);
        }

        [HttpPut]
        [Route("EditSectorType")]
        public async Task<IActionResult> EditSectorType(int sectorTypeID, SectorTypeModel model)
        {
            //return await _adminservice.EditSectorType(sectorTypeID, model);
            if(sectorTypeID != model.TypeID)
            {
                return BadRequest();
            }
            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (sectorType == null)
            {
                return NotFound();
            }

            sectorType.Title = model.Title;
            sectorType.Description = model.Description;
            sectorType.TypeID = model.TypeID;

            return Ok(sectorType);

        }

        [HttpGet]
        [Route("GetSectorType")]
        public async Task<ActionResult<SectorTypeModel>> GetSectorType(int sectorTypeID)
        {
            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(sectorType == null)
            {
                return NotFound();
            }
            return sectorType;
            //return EmployeeEntityToModel(employee);
        }

        [HttpDelete]
        [Route("DeleteSectorType")]
        public async Task<IActionResult> DeleteSectorType(int sectorTypeID)
        {
          // return await _adminservice.DeleteSectorType(sectorTypeID);

            var sectorType = SectorTypes.Find(x => x.TypeID == sectorTypeID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (sectorType == null)
            {
                return NotFound();
            }
            
            SectorTypes.Remove(sectorType);
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok();
        }

        [HttpPost]
        [Route("CreateTemplate")]
        public async Task<IActionResult> CreateTemplate(TemplateModel model)
        {
            // return await _adminservice.CreateTemplate(model);


            templateModels.Add(model);
            TemplateEntity entity = new TemplateEntity
            {
                TemplateID = model.TemplateID,
                Title = model.Title,
                Description = model.Description
            };
            //_databaseContext.Employees.Add(entity);

            return CreatedAtAction(
                nameof(GetTemplate),
                new { TemplateID = model.TemplateID },
                model);
        }

        [HttpGet]
        [Route("GetTemplate")]
        public async Task<ActionResult<TemplateModel>> GetTemplate(int templateID)
        {
            //return await _adminservice.GetTemplate(templateID);


            var template = templateModels.Find(x => x.TemplateID == templateID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(template == null)
            {
                return NotFound();
            }
            return template;
            //return EmployeeEntityToModel(employee);
        }


        [HttpGet]
        [Route("GetSectorsInTemplate")]
        public async Task<ActionResult<IEnumerable<SectorTypeModel>>> GetSectorsInTemplate(int templateID)
        {
            //return await _adminservice.GetTemplate(templateID);


            var template = templateModels.Find(x => x.TemplateID == templateID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if(template == null)
            {
                return NotFound();
            }
            return template.SectorTypes;
            //return EmployeeEntityToModel(employee);
        }


        [HttpPut]
        [Route("EditTemplate")]
        public async Task<IActionResult> EditTemplate(int templateID, TemplateModel model)
        {
            // return await _adminservice.EditTemplate(templateID, model);

            //return await _adminservice.EditSectorType(sectorTypeID, model);
            if(templateID != model.TemplateID)
            {
                return BadRequest();
            }
            var template = templateModels.Find(x => x.TemplateID == templateID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (template == null)
            {
                return NotFound();
            }

            template.Title = model.Title;
            template.TemplateID = model.TemplateID;
            template.Description = model.Description;

            return Ok(template);
        }

        [HttpPost]
        [Route("AssignSectorType")]
        public async Task<HttpResponseMessage> AssignSectorType(int templateID, int sectorTypeID)
        {
            return await _adminservice.AssignSectorType(templateID, sectorTypeID);
        }

        [HttpDelete]
        [Route("DeleteTemplate")]
        public async Task<IActionResult> DeleteTemplate(int templateID)
        {
            // return await _adminservice.DeleteTemplate(templateID);

            var template = templateModels.Find(x => x.TemplateID== templateID);
            //var employee = await _databaseContext.Employees.FindAsync(EID);

            if (template == null)
            {
                return NotFound();
            }
            
            templateModels.Remove(template);
            //_databaseContext.Employees.Remove(employee);
            //return NoContent();
            return Ok();
        }


        private bool EmployeeExists(long EID)
        {
            return Employees.Any(e => e.EID == EID);
        }

        private static EmployeeModel EmployeeEntityToModel(EmployeeEntity entity) =>
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
