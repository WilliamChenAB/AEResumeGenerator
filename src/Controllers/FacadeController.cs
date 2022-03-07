using System;
using ae_resume_api.Authentication;
using ae_resume_api.Admin;
using ae_resume_api.DBContext;
using ae_resume_api.Facade;
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


namespace ae_resume_api.Controllers
{
	[Route("Facade")]
	[ApiController]
	public class FacadeController : ControllerBase
	{
		private readonly IFacadeService _facadeservice;
		public FacadeController(IFacadeService facadeservice)
		{
			_facadeservice = facadeservice;
		}

		[HttpPost]
        [Route("NewResume")]
		public async Task<HttpResponseMessage> NewResume([FromBody] ResumeModel model, int EID)
		{
			return await _facadeservice.NewResume(model, EID);
		}

		[HttpPost]
		[Route("NewSector")]
		public async Task<HttpResponseMessage> NewSector(SectorModel model)
		{
			return await _facadeservice.NewSector(model);
		}

		[HttpGet]
		[Route("GetSector")]
		public async Task<HttpResponseMessage> GetSector(int SID)
		{
			return await _facadeservice.GetSector(SID);
		}

		[HttpDelete]
		[Route("DeleteResume")]
		public async Task<HttpResponseMessage> DeleteResume(int RID)
		{
			return await _facadeservice.DeleteResume(RID);
		}

		[HttpDelete]
		[Route("DeleteSector")]
		public async Task<HttpResponseMessage> DeleteSector(int SID, int RID)
		{
			return await _facadeservice.DeleteSector(SID, RID);
		}

		[HttpPut]
		[Route("EditSector")]
		public async Task<HttpResponseMessage> EditSector(int SID, SectorModel model)
		{
			return await _facadeservice.EditSector(SID, model);
		}

		[HttpPut]
		[Route("EditResume")]
		public async Task<HttpResponseMessage> EditResume(int RID, int SID,  SectorModel model)
		{
			return await _facadeservice.EditResume(RID, SID, model);
		}

		[HttpGet]
		[Route("ExportResume")]
		public async Task<HttpResponseMessage> ExportResume(int RID)
		{
			return await _facadeservice.ExportResume(RID);
		}

		[HttpGet]
		[Route("SearchResume")]
		public async Task<HttpResponseMessage> SearchResume(string filter)
		{
			return await _facadeservice.SearchResume(filter);
		}
	}
}

