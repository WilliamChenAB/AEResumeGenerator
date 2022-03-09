using Microsoft.AspNetCore.Mvc;
using System;

namespace ae_resume_api.Facade
{
	public interface IFacadeService
	{
        Task<HttpResponseMessage> NewResume(ResumeModel model, int EID);
        Task<HttpResponseMessage> NewSector(SectorModel model);
        Task<HttpResponseMessage> GetSector(int SID);
        Task<HttpResponseMessage> DeleteResume(int RID);
        Task<HttpResponseMessage> DeleteSector(int SID, int RID);
        Task<HttpResponseMessage> EditSector(int SID, SectorModel model);
        Task<HttpResponseMessage> EditResume(int RID, int SID, SectorModel model);
        Task<HttpResponseMessage> ExportResume(int RID);
        Task<HttpResponseMessage> SearchResume(string filter);
    }
}
