using ae_resume_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.Extensions.Configuration;
using ae_resume_api.Tests;
using System.IO;
using IdentityModel.Client;

namespace ae_resume_api.Controllers.Tests
{

    public class AttributesControllerTests : APITest
    {

        public AttributesControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public void NewWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void CopyResumeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);


            var stringContent = new StringContent("{\"TemplateID\":1, \"EID\":3, \"WID\":2}");
            var response = await _client.PostAsync("/Attributes/CopyResume?RID=31&WID=4", stringContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void DeleteWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetResumesForWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void CreateTemplateRequestTest()
        { 
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            
            var stringContent = new StringContent("{\"TemplateID\":1, \"EID\":eebc735b-b277-43f0-a882-8391177dd93a, \"WID\":2}");
            var response = await _client.PostAsync("/Attributes/CreateTemplateRequest?TemplateID=1&EID=eebc735b-b277-43f0-a882-8391177dd93a&WID=4", stringContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void AddEmptyResumeToWorkspace()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);


            var stringContent = new StringContent("");
            var response = await _client.PostAsync("/Attributes/AddEmptyResumeToWorkspace?WID=4&resumeName=blankTest&EID=eebc735b-b277-43f0-a882-8391177dd93a", stringContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
    }
}