using ae_resume_api.Models;
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

        private class RegisterModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        private static readonly string EMPLOYEE_PASSWORD = "Abcd1234";
        private static readonly string EMPLOYEE_EMAIL = "email@email.com";

        /// <summary>
        /// Clean the tables and load in the test data
        /// </summary>
        [HttpPost]
        [Route("LoadTestData")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadTestData()
        {

            _databaseContext.Sector.DeleteAll();
            _databaseContext.TemplateSector.DeleteAll();
            _databaseContext.Resume.DeleteAll();
            _databaseContext.Workspace.DeleteAll();
            _databaseContext.Template.DeleteAll();
            _databaseContext.SectorType.DeleteAll();
            _databaseContext.Employee.DeleteAll();

            //Create logins on the IDP and populate user accounts
            var httpClient = new HttpClient();

            for (int i = 1; i <= 9; i++)
            {
                var username = "user" + i;

                var url = configuration["Authority"] + "/Identity/Account/RegisterNoVerify";
                var creds = new RegisterModel()
                {
                    UserName = username,
                    Password = EMPLOYEE_PASSWORD
                };

                var response = await httpClient.PostAsJsonAsync(url, creds);
                if (!response.IsSuccessStatusCode) return BadRequest(response.Content);

                var content = await response.Content.ReadFromJsonAsync<Guid>();
                Access access = (Access)(i / 3);

                EmployeeEntity employee = new EmployeeEntity
                {
                    EmployeeId = content,
                    Email = EMPLOYEE_EMAIL,
                    Access = access,
                    Name = "User " + i,
                    JobTitle = access.ToString() + " User"
                };

                _databaseContext.Employee.Add(employee);
            }

            await _databaseContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("LoadDefaultAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadDefaultAdmin()
        {
            var url = configuration["Authority"] + "/Identity/Account/RegisterNoVerify";
            var creds = new RegisterModel()
            {
                UserName = "admin",
                Password = "pbVxh!6sE1rgdTfQ"
            };

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync(url, creds);
            if (!response.IsSuccessStatusCode) return BadRequest(response.Content);

            var content = await response.Content.ReadFromJsonAsync<Guid>();

            EmployeeEntity employee = new EmployeeEntity
            {
                EmployeeId = content,
                Email = EMPLOYEE_EMAIL,
                Access = Access.SystemAdmin,
                Name = "Administrator",
                JobTitle = "Default System Administrator"
            };

            try
            {
                _databaseContext.Employee.Add(employee);
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("Identity")]
        public IActionResult Identity()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
