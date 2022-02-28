using Microsoft.AspNetCore.Mvc;
using System;

namespace ae_resume_api.Admin
{
	public interface IAdminService
	{
        Task<HttpResponseMessage> CreateEmployee(EmployeeModel model);
        Task<HttpResponseMessage> DeleteEmployee(long EID);
        Task<HttpResponseMessage> EditEmployee(long EID, EmployeeModel employeeModel);
        Task<HttpResponseMessage> AssignEmployeeAccess(long EID, string access);

    }
}
