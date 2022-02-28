using ae_resume_api.Authentication;
using ae_resume_api.Admin;
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
    [Route("[Admin]")]
    [ApiController]
    public class AdminController : ControllerBase
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
        public async Task<HttpResponseMessage> EditEmployee(long Eid, EmployeeModel employeeModel)
        {
            //return BadRequest("Not setup");
            return await _adminservice.EditEmployee(Eid, employeeModel);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<HttpResponseMessage> DeleteEmployee(long EID)
        {
            return await _adminservice.DeleteEmployee(EID);
        }

        [HttpPost]
        [Route("AssignAccess")]
        public async Task<HttpResponseMessage> AssignAccess(long EID, string access)
        {
            return await _adminservice.AssignEmployeeAccess(EID, access);
        }

        [HttpPost]
        [Route("NewSectorType")]
        public async Task<IActionResult> NewSectorType()
        {
            return BadRequest("Not setup");
        }

        [HttpPut]
        [Route("EditSectorType")]
        public async Task<IActionResult> EditSectorType()
        {
            return BadRequest("Not setup");
        }

        [HttpDelete]
        [Route("DeleteSectorType")]
        public async Task<IActionResult> DeleteSectorType()
        {
           return BadRequest("Not setup");
        }

        [HttpPost]
        [Route("CreateTemplate")]
        public async Task<IActionResult> CreateTemplate()
        {
            return BadRequest("Not setup");
        }

        [HttpGet]
        [Route("GetTemplate")]
        public async Task<IActionResult> GetTemplate()
        {
            return BadRequest("Not setup");
        }

        [HttpPut]
        [Route("EditTemplate")]
        public async Task<IActionResult> EditTemplate()
        {
            return BadRequest("Not setup");
        }

        [HttpPost]
        [Route("AssignSectorType")]
        public async Task<IActionResult> AssignSectorType()
        {
            return BadRequest("Not setup");
        }

        [HttpDelete]
        [Route("DeleteTemplate")]
        public async Task<IActionResult> DeleteTemplate()
        {
            return BadRequest("Not setup");
        }
    }
}
