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

        public AdminController(IAdminService adminservice)
        {
            _adminservice = adminservice;
        }

        [HttpPost]
        [Route("NewEmployee")]
        public async Task<HttpResponseMessage> NewEmployee([FromBody] EmployeeModel model)
        {          
            return await _adminservice.CreateEmployee(model);
        }

        [HttpPut]
        [Route("EditEmployee")]
        public async Task<HttpResponseMessage> EditEmployee(int Eid, EmployeeModel employeeModel)
        {
            //return BadRequest("Not setup");
            return await _adminservice.EditEmployee(Eid, employeeModel);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<HttpResponseMessage> DeleteEmployee(int EID)
        {
            return await _adminservice.DeleteEmployee(EID);
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
    }
}
