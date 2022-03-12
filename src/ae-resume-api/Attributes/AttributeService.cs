using ae_resume_api.Authentication;
using ae_resume_api.DBContext;
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

namespace ae_resume_api.Attributes
{
    public class AttributeService : IAttributeService
    {
        readonly DatabaseContext _databaseContext;

        public AttributeService(DatabaseContext dbContext)
        {
            _databaseContext = dbContext;
        }

        public async Task<HttpResponseMessage> NewWorkspace(WorkspaceModel model)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> GetWorkspace(int WID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> CopyResume(int EID, int WID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> DeleteWorkspace(int WID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Delete, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> GetResumes(int WID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
        public async Task<HttpResponseMessage> CreateTemplateRequest(int TemplateID, int EID)
        {
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            // TODO: implement

            returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Not implemented");

            return await Task.FromResult(returnMessage);
        }
    }
}
