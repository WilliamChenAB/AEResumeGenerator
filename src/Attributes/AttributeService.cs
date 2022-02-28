using ae_resume_api.Authentication;
using ae_resume_api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
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
using Microsoft.EntityFrameworkCore;

public class AttributeService : IAttributeService
{
	readonly DatabaseContext _databaseContext;

	public AttributeService(DatabaseContext dbContext)
	{
		_databaseContext = dbContext;
	}
}
