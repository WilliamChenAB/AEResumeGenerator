using ae_resume_api.Authentication;
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
 

namespace ae_resume_api.src.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("NewEmployee")]
        public async Task<IActionResult> NewEmployee()
        {
            
            await _context.SaveChangesAsync();


            try
            {
                //var response = new HttpResponseMessage(HttpStatusCode.OK);

                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);


                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

    }
}
