using System;
using ae_resume_api.Attributes;
using ae_resume_api.DBContext;
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


namespace ae_resume_api.Controllers
{
	[Route("Attributes")]
	[ApiController]
	public class AtrributesController : ControllerBase
	{
		private readonly IAttributeService _attributeservice;
		public AtrributesController(IAttributeService attributeservice)
		{
			_attributeservice = attributeservice;
		}

		[HttpPost]
		[Route("NewWorkspace")]
		public async Task<HttpResponseMessage> NewWorkspace([FromBody] WorkspaceModel model)
		{
			return await _attributeservice.NewWorkspace(model);
		}

		[HttpGet]
		[Route("GetWorkspace")]
		public async Task<HttpResponseMessage> GetWorkspace(int WID)
		{
			return await _attributeservice.GetWorkspace(WID);
		}

		[HttpPost]
		[Route("CopyResume")]
		public async Task<HttpResponseMessage> CopyResume(int EID, int WID)
		{
			return await _attributeservice.CopyResume(EID, WID);
		}

		[HttpDelete]
		[Route("DeleteWorkspace")]
		public async Task<HttpResponseMessage> DeleteWorkspace(int WID)
		{
			return await _attributeservice.DeleteWorkspace(WID);
		}

		[HttpGet]
		[Route("GetResumes")]
		public async Task<HttpResponseMessage> GetResumes(int WID)
		{
			return await _attributeservice.GetResumes(WID);
		}

		[HttpPost]
		[Route("CreateTemplateRequest")]
		public async Task<HttpResponseMessage> CreateTemplateRequest(int TemplateID, int EID)
		{
			return await _attributeservice.CreateTemplateRequest(TemplateID, EID);
		}
	}
}

