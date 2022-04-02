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
            _databaseContext.Sector.ReseedAll();

            _databaseContext.Resume.DeleteAll();
            _databaseContext.Resume.ReseedAll();

            _databaseContext.TemplateSector.DeleteAll();

            _databaseContext.Template.DeleteAll();
            _databaseContext.Template.ReseedAll();

            _databaseContext.Workspace.DeleteAll();
            _databaseContext.Workspace.ReseedAll();

            _databaseContext.SectorType.DeleteAll();
            _databaseContext.SectorType.ReseedAll();

            _databaseContext.Employee.DeleteAll();


            //Create logins on the IDP and populate user accounts
            var httpClient = new HttpClient();


            await LoadTestEmployees(httpClient);
            await LoadTestTemplates(_databaseContext);
            await LoadTestWorkspaces(_databaseContext);
            await LoadTestResumes(_databaseContext);

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

        private async Task<IActionResult> LoadTestEmployees(HttpClient httpClient)
        {
            for (int i = 0; i < 9; i++)
            {
                var username = "user" + (i + 1);

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
                    Name = "User " + (i + 1),
                    JobTitle = access.ToString() + " User"
                };

                _databaseContext.Employee.Add(employee);
            }

            await _databaseContext.SaveChangesAsync();
            return Ok();
        }

        private static async Task LoadTestResumes(DatabaseContext databaseContext)
        {
            // Foreach employee create a resume with the test sector types
            var employees = await databaseContext.Employee.ToListAsync();
            foreach (var employee in employees)
            {
                var resume = await databaseContext.Resume.AddAsync(new ResumeEntity
                {
                    Creation_Date = ControllerHelpers.CurrentTimeAsString(),
                    Last_Edited = ControllerHelpers.CurrentTimeAsString(),
                    Status = Status.Regular,
                    Name = employee.Name + " Resume",
                    EmployeeId = employee.EmployeeId,
                    WorkspaceId = 1,
                    TemplateId = 1
                });
                await databaseContext.SaveChangesAsync();

                // Add a sector of each type
                for (int i = 1; i < 3; i++)
                {
                    await databaseContext.Sector.AddAsync(new SectorEntity
                    {
                        Creation_Date = ControllerHelpers.CurrentTimeAsString(),
                        Last_Edited = ControllerHelpers.CurrentTimeAsString(),
                        Content = "test sector",
                        Division = "test division",
                        Image = "test image",
                        ResumeId = resume.Entity.ResumeId,
                        TypeId = i
                    });
                }
            }
            await databaseContext.SaveChangesAsync();

        }

        private static async Task LoadTestTemplates(DatabaseContext databaseContext)
        {
            // Create template
            await databaseContext.Template.AddAsync(new TemplateEntity
            {
                Title = "test template",
                Description = "Template for testing",
                Last_Edited = ControllerHelpers.CurrentTimeAsString()
            });

            await databaseContext.SaveChangesAsync();

            // Load sector types
            // Populate sector types
            await databaseContext.SectorType.AddAsync(new SectorTypeEntity
            {
                Title = "Education",
                Description = "Education sector"
            });

            await databaseContext.SectorType.AddAsync(new SectorTypeEntity
            {
                Title = "Work Experience",
                Description = "Work Experience sector"
            });

            await databaseContext.SectorType.AddAsync(new SectorTypeEntity
            {
                Title = "Summary",
                Description = "Summary sector"
            });
            await databaseContext.SaveChangesAsync();

            // Assign sector types to Template
            await databaseContext.TemplateSector.AddRangeAsync(new List<TemplateSectorEntity>
            {
                new TemplateSectorEntity{TemplateId = 1, TypeId = 1 },
                new TemplateSectorEntity{TemplateId = 1, TypeId = 2 },
                new TemplateSectorEntity{TemplateId = 1, TypeId = 3 }
            });
            await databaseContext.SaveChangesAsync();
        }

        private static async Task LoadTestWorkspaces(DatabaseContext databaseContext)
        {
            var employeeId = databaseContext.Employee.FirstAsync().Result.EmployeeId;

            // Create Workspace
            await databaseContext.Workspace.AddAsync(new WorkspaceEntity
            {
                Name = "test workspace",
                Proposal_Number = "test1",
                Division = "test division",
                Creation_Date = ControllerHelpers.CurrentTimeAsString(),
                EmployeeId = employeeId
            });
            await databaseContext.SaveChangesAsync();
        }
    }
}
