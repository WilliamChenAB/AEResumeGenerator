using Microsoft.AspNetCore.Mvc;
using System;

namespace ae_resume_api.Admin
{
	public interface IAdminService
	{
        Task<HttpResponseMessage> CreateEmployee(EmployeeModel model);
    }
}
