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

namespace ae_resume_api.Controllers.Tests
{
    
    public class ResumeControllerTests: APITest
    {
        // TODO: assign for testing
        public string testEmployeeId = "";

        public ResumeControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public async void NewResumeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.PostAsync("/Resume/NewPersonal?TemplateId=1&resumeName=test", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void NewTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.PostAsync("/Resume/New?TemplateId=1&resumeName=test&EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void DeleteTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.DeleteAsync("/Resume/Delete?ResumeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Resume/Get?ResumeId=2");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void EditTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.PutAsync("/Resume/Edit?ResumeId=2", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllForEmployeeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Resume/GetAllForEmployee?EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetPersonalTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Resume/GetPersonal");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetPersonalForEmployeeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Resume/GetPersonalForEmployee?EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
    }
}