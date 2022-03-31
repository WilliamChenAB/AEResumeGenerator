using ae_resume_api.DBContext;
using ae_resume_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ae_resume_api.Controllers
{

    [Route("Employee")]
    [ApiController]
    public class EmployeeController : Controller
    {

        readonly DatabaseContext _databaseContext;
        private readonly IConfiguration configuration;

        public EmployeeController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _databaseContext = dbContext;
            this.configuration = configuration;
        }


        /// <summary>
        /// Edit an Employee
        /// </summary>
        [HttpPut]
        [Route("EditOwnBio")]
        public async Task<IActionResult> EditOwnBio(string name, string jobTitle)
        {
            var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
            if (EmployeeId == null) return NotFound();
            var employee = await _databaseContext.Employee.FindAsync(Guid.Parse(EmployeeId));
            if (employee == null) return NotFound("Employee not found");

            employee.Name = name;
            employee.JobTitle = jobTitle;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(employee);
        }

        /// <summary>
        /// Delete an Employee
        /// </summary>
        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> Delete(string EmployeeId)
        {
            var employee = await _databaseContext.Employee.FindAsync(Guid.Parse(EmployeeId));

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _databaseContext.Employee.Remove(employee);
            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return Ok();
        }

        /// <summary>
        /// Get self
        /// </summary>
        [HttpGet]
        [Route("GetSelf")]
        public async Task<ActionResult<EmployeeModel>> GetSelf()
        {
            var EmployeeId = User.FindFirst(configuration["TokenIDClaimType"])?.Value;
            if (EmployeeId == null) return NotFound();
            return await Get(EmployeeId);
        }

        /// <summary>
        /// Get an Employee from their EmployeeId
        /// </summary>
        [HttpGet]
        [Route("Get")]
        [Authorize(Policy = "SA")]
        public async Task<ActionResult<EmployeeModel>> Get(string EmployeeId)
        {
            var employee = await _databaseContext.Employee.FindAsync(Guid.Parse(EmployeeId));

            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            return ControllerHelpers.EmployeeEntityToModel(employee);
        }

        /// <summary>
        /// Get all Employees
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<EmployeeModel> GetAll()
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
        [Authorize(Policy = "SA")]
        public async Task<IActionResult> AssignAccess(string EmployeeId, Access access)
        {
            var employee = await _databaseContext.Employee.FindAsync(Guid.Parse(EmployeeId));

            // Check if the employee already exists
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            employee.Access = access;


            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

            return Ok(employee);

        }

    }

}
