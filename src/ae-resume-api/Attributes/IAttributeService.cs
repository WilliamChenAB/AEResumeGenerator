using Microsoft.AspNetCore.Mvc;
using System;

namespace ae_resume_api.Attributes
{
	public interface IAttributeService
	{
        Task<HttpResponseMessage> NewWorkspace(WorkspaceModel model);
        Task<HttpResponseMessage> GetWorkspace(int WID);
        Task<HttpResponseMessage> CopyResume(int EID, int WID);
        Task<HttpResponseMessage> DeleteWorkspace(int WID);
        Task<HttpResponseMessage> GetResumes(int WID);
        Task<HttpResponseMessage> CreateTemplateRequest(int TemplateID, int EID);

    }
}
