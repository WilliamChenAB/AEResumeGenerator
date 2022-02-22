using ae_resume_api.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
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

namespace ae_resume_api.Admin
{
	public class AdminService : IAdminService
	{
		readonly DatabaseContext _databaseContext;
		public AdminService(DatabaseContext dbContext)
		{
			_databaseContext = dbContext;
		}
		public async Task<HttpResponseMessage> CreateEmployee(EmployeeModel model)
        {
            EmployeeEntity employee = new EmployeeEntity();
            employee.EID = model.EID;
            employee.Username = model.Username;
            employee.Password = model.Password;
            employee.Name = model.Name;
            employee.Email = model.Email;

           
            HttpResponseMessage returnMessage = new HttpResponseMessage();

            try
            {
                // add employee to database
                _databaseContext.Employees.Add(employee);
                await _databaseContext.SaveChangesAsync();
                string message = ($"Employee Created - {employee.EID}");
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, message);
            }
            catch (Exception ex)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, ex.ToString());
            }


            return await Task.FromResult(returnMessage);
        }
	}
}
