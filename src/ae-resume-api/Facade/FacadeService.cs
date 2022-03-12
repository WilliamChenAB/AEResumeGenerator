using ae_resume_api.Facade;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using ae_resume_api.DBContext;
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


namespace ae_resume_api.Facade
{
	public class FacadeService : IFacadeService
	{
		readonly DatabaseContext _databaseContext;

		public FacadeService(DatabaseContext dbContext)
		{
			_databaseContext = dbContext;
		}


		public async Task<HttpResponseMessage> NewResume(ResumeModel model, int EID)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> NewSector(SectorModel model)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> GetSector(int SID)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> DeleteResume(int RID)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Delete, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> DeleteSector(int SID, int RID)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Delete, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> EditSector(int SID, SectorModel model)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> EditResume(int RID, int SID, SectorModel model)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Put, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> ExportResume(int RID)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
		public async Task<HttpResponseMessage> SearchResume(string filter)
        {
			HttpResponseMessage returnMessage = new HttpResponseMessage();
			// TODO: implement

			returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
			returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Not implemented");

			return await Task.FromResult(returnMessage);
		}
	}
}
