using System;
using ae_resume_api.Authentication;
using ae_resume_api.Admin;
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
	[Route("[Facade]")]
	[ApiController]
	public class FacadeController : ControllerBase
	{
		private readonly IFacadeController _facadeservice;
		public FacadeController(IFacadeController facadeservice)
		{
			_facadeservice = facadeservice;
		}
	}
}

