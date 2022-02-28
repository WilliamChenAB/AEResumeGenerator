using System;
using ae_resume_api.Attributes;
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
	[Route("[Attributes]")]
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
		public async Task<IActionResult> NewWorkspace([FromBody] string s)
		{
			return BadRequest("Not setup");
		}

		[HttpGet]
		[Route("GetWorkspace")]
		public async Task<IActionResult> GetWorkspace(long WID)
		{
			return BadRequest("Not setup");
		}

		[HttpPost]
		[Route("CopyResume")]
		public async Task<IActionResult> CopyResume(long RID)
		{
			return BadRequest("Not setup");
		}

		[HttpDelete]
		[Route("DeleteWorkspace")]
		public async Task<IActionResult> DeleteResume(long WID)
		{
			return BadRequest("Not setup");
		}

		[HttpGet]
		[Route("GetResumes")]
		public async Task<IActionResult> GetResumes(long WID)
		{
			return BadRequest("Not setup");
		}

		[HttpPost]
		[Route("CreateTemplateRequest")]
		public async Task<IActionResult> CreateTemplateRequest(long TID)
		{
			return BadRequest("Not setup");
		}
	}
}

