//using IdentityModel;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ae_resume_api.Authorization
{
    public class AccessHandler : AuthorizationHandler<AccessRequirement>
    {
        private DatabaseContext _databaseContext;
        private IConfiguration _configuration;

        public AccessHandler(DatabaseContext database, IConfiguration config)
        {
            _databaseContext = database;
            _configuration = config;
        }

        protected override async Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          AccessRequirement requirement)
        {
            var user = context.User;
            var userID = user.FindFirst(_configuration["TokenIDClaimType"])?.Value;

            if (userID == null) {
                context.Fail();
                return;
            }

            var employee = await _databaseContext.Employee.FindAsync(userID);

            if (employee == null) {
                var chngEntity = _databaseContext.Employee.Add(new EmployeeEntity
                {
                    EID = userID,
                    Access = Access.Employee
                });
                await _databaseContext.SaveChangesAsync();

                employee = chngEntity.Entity;
            }

            if (employee.Access >= requirement.Access) context.Succeed(requirement);
            else context.Fail();

        }
    }
}
