using ae_resume_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Mvc.Testing;
using ae_resume_api.Models;
using Xunit;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using ae_resume_api.Tests;
using System.IO;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace ae_resume_api.Controllers.Tests
{
    public class WorkspaceControllerTests : APITest
    {
        public WorkspaceControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public async void NewTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Workspace/New?division=water&proposalNumber=1&name=WorkspaceName");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Workspace/Get?WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void DeleteTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.DeleteAsync("/Workspace/Delete?WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetPersonalTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Workspace/GetPersonal");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetResumesTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Workspace/GetResumes?WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void CreateTemplateRequestTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Workspace/CreateTemplateRequest?TemplateId=1&EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16&WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void AddEmptyResumeTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Workspace/AddEmptyResume?WorkspaceId=1&TemplateId=1&EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void CopyResumeTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Workspace/CopyResume?ResumeId=1&WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void SubmitResumeTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Workspace/SubmitResume?ResumeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllSectorTypesTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Workspace/GetAllSectorTypes?WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
    }
}