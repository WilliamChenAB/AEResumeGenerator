using Microsoft.AspNetCore.Mvc;
using System;

namespace ae_resume_api.Admin
{
	public interface IAdminService
	{
        Task<HttpResponseMessage> CreateEmployee(EmployeeModel model);
        Task<HttpResponseMessage> DeleteEmployee(int EID);
        Task<ActionResult<EmployeeModel>> GetEmployee(int EID);
        Task<HttpResponseMessage> EditEmployee(int EID, EmployeeModel employeeModel);
        Task<HttpResponseMessage> AssignEmployeeAccess(int EID, string access);
        Task<HttpResponseMessage> NewSectorType(SectorTypeModel model);
        Task<HttpResponseMessage> EditSectorType(int sectorTypeID, SectorTypeModel model);
        Task<HttpResponseMessage> DeleteSectorType(int sectorTypeID);
        Task<HttpResponseMessage> CreateTemplate(TemplateModel model);
        Task<HttpResponseMessage> GetTemplate(int templateID);
        Task<HttpResponseMessage> EditTemplate(int templateID, TemplateModel model);
        Task<HttpResponseMessage> AssignSectorType(int templateID, int sectorTypeID);
        Task<HttpResponseMessage> DeleteTemplate(int templateID);

    }
}
