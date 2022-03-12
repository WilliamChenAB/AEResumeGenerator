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
        public List<EmployeeModel> Employees = new List<EmployeeModel>();

        public AdminController(IAdminService adminservice)
        {
            _adminservice = adminservice;
            this.Employees = new List<EmployeeModel>();
            Employees.Add(new EmployeeModel { 
                EID = 5,
                Name = "James",
                Email = "email",
                Username = "James",
                Password = "password"
            });
        }

        [HttpPost]
        [Route("NewEmployee")]
        public async Task<ActionResult<EmployeeModel>> NewEmployee([FromBody] EmployeeModel model)
        {
            //return await _adminservice.CreateEmployee(model);

            Employees.Add(model);

            return CreatedAtAction(
                nameof(GetEmployee),
                new { EID = model.EID },
                model);
        }

        [HttpPut]
        [Route("EditEmployee")]
        public async Task<IActionResult> EditEmployee(int Eid, EmployeeModel employeeModel)
        {
            //return await _adminservice.EditEmployee(Eid, employeeModel);

            if(Eid != employeeModel.EID)
            {
                return BadRequest();
            }
            var employee = Employees.Find(x => x.EID == Eid);
            if(employee == null)
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
            if(employee == null)
            {
                return NotFound();
            }
            Employees.Remove(employee);
            //return NoContent();
            return Ok();
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<ActionResult<EmployeeModel>> GetEmployee(int EID)
        {
            //return await _adminservice.GetEmployee(EID);

            var employee = Employees.Find(x => x.EID == EID);
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
        public async Task<HttpResponseMessage> NewSectorType(SectorTypeModel model)
        {
            return await _adminservice.NewSectorType(model);
        }

        [HttpPut]
        [Route("EditSectorType")]
        public async Task<HttpResponseMessage> EditSectorType(int sectorTypeID, SectorTypeModel model)
        {
            return await _adminservice.EditSectorType(sectorTypeID, model);
        }

        [HttpDelete]
        [Route("DeleteSectorType")]
        public async Task<HttpResponseMessage> DeleteSectorType(int sectorTypeID)
        {
           return await _adminservice.DeleteSectorType(sectorTypeID);
        }

        [HttpPost]
        [Route("CreateTemplate")]
        public async Task<HttpResponseMessage> CreateTemplate(TemplateModel model)
        {
            return await _adminservice.CreateTemplate(model);
        }

        [HttpGet]
        [Route("GetTemplate")]
        public async Task<HttpResponseMessage> GetTemplate(int templateID)
        {
            return await _adminservice.GetTemplate(templateID);
        }

        [HttpPut]
        [Route("EditTemplate")]
        public async Task<HttpResponseMessage> EditTemplate(int templateID, TemplateModel model)
        {
            return await _adminservice.EditTemplate(templateID, model);
        }

        [HttpPost]
        [Route("AssignSectorType")]
        public async Task<HttpResponseMessage> AssignSectorType(int templateID, int sectorTypeID)
        {
            return await _adminservice.AssignSectorType(templateID, sectorTypeID);
        }

        [HttpDelete]
        [Route("DeleteTemplate")]
        public async Task<HttpResponseMessage> DeleteTemplate(int templateID)
        {
            return await _adminservice.DeleteTemplate(templateID);
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
