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

        /// <summary>
        /// Clean the tables and load in the test data
        /// </summary>
        [HttpPost]
        [Route("LoadTestData")]
        [Authorize (Policy = "SA")]
        public async Task<IActionResult> LoadTestData()
        {
            // Careful, don't need SaveChanges() call, this happens immediately

            // Not truncating employees currently
            //_databaseContext.Employee.Truncate();
            _databaseContext.Resume.Truncate();
            _databaseContext.Sector.Truncate();
            _databaseContext.SectorType.Truncate();
            _databaseContext.Template.Truncate();
            _databaseContext.TemplateSector.Truncate();
            _databaseContext.Workspace.Truncate();

            EmployeeEntity employee1 = new EmployeeEntity
            {
                EmployeeId = Guid.NewGuid(),
                Access = Access.SystemAdmin,
                Name = "James",
                Email = "Email",
                JobTitle = "Utility Coordinator"
            };

            return BadRequest("Not implemented");
        }

        [HttpGet]
        [Route("Identity")]
        public IActionResult Identity()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
