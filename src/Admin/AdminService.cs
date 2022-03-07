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
using Microsoft.EntityFrameworkCore;
using ae_resume_api.DBContext;

namespace ae_resume_api.Admin
{
    public class AdminService : IAdminService
    {
        readonly DatabaseContext _databaseContext;
        public AdminService(DatabaseContext dbContext)
        {
            _databaseContext = dbContext;
        }
        /// <summary>
        /// Create an employee from passed in request data
        /// </summary>
        /// <returns>Response message with either success or fail</returns>
		public async Task<HttpResponseMessage> CreateEmployee(EmployeeModel model)
        {
            EmployeeEntity employee = new EmployeeEntity();
            Console.WriteLine("Creating employee: " + model.Name);
            employee.EID = model.EID;
            employee.Username = model.Username;
            employee.Password = model.Password;
            employee.Name = model.Name;
            employee.Email = model.Email;

            // Build response message
            HttpResponseMessage returnMessage = new HttpResponseMessage();

            try
            {
                // Add Employee to database
                _databaseContext.Employees.Add(employee);
                await _databaseContext.SaveChangesAsync();
                Console.WriteLine("Employee count:" + _databaseContext.Employees.Count());
                string message = ($"Employee Created - {employee.EID}");
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, ex.ToString());
            }


            return await Task.FromResult(returnMessage);
        }
        /// <summary>
        /// Edit an Employee specified by the provided EID with the model information provided
        /// </summary>        
        /// <returns></returns>
        public async Task<HttpResponseMessage> EditEmployee(int EID, EmployeeModel model)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();

            // Check that the Employee we want to edit matches the EmployeeModel
            if (EID != model.EID)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "No edit");
            }

            // Find the Employee in the database to edit
            // Error if no Employee found
            var employee = await _databaseContext.Employees.FindAsync(EID);
            if (employee == null)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Employee not found");
            }

            
            // TODO: implement

            //try
            //{
            //    await _databaseContext.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException) when (!EmployeeExists(EID))
            //{
            //    return NotFound();
            //}
            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        /// <summary>
        /// Delete an employee
        /// </summary>
        public async Task<HttpResponseMessage> DeleteEmployee(int EID)
        {

            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Delete, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        /// <summary>
        /// Change the access for a given employee
        /// </summary>
        /// <param name="access">New access for an employee</param>
        public async Task<HttpResponseMessage> AssignEmployeeAccess(int EID, string access)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Not implemented");

            return await Task.FromResult(returnMessage);
        }

        public async Task<HttpResponseMessage> NewSectorType(SectorTypeModel model)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

            return await Task.FromResult(returnMessage);
        }

        public async Task<HttpResponseMessage> EditSectorType(int sectorTypeID, SectorTypeModel model)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Not implemented");

            return await Task.FromResult(returnMessage);
        }

        public async Task<HttpResponseMessage> DeleteSectorType(int sectorTypeID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Delete, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> CreateTemplate(TemplateModel model)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> GetTemplate(int templateID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> EditTemplate(int templateID, TemplateModel model)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> AssignSectorType(int templateID, int sectorTypeID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> DeleteTemplate(int templateID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Delete, "Not implemented");

            return await Task.FromResult(returnMessage);
        }

        /// <summary>
        /// Check if employee exists
        /// </summary>
        private bool EmployeeExists(long EID)
        {
            return _databaseContext.Employees.Any(e => e.EID == EID);
        }
    }


}
