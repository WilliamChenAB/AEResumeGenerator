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
    
    public class SectorControllerTests : APITest
    {
        // TODO: assign for testing
        public string testEmployeeId = "";

        public SectorControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public async void NewTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            SectorModel myClass = new SectorModel
            {
                Content = "test",
                Image = "/Imagelink",
                Division = "Water",
                ResumeId = 1,
                TypeId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(myClass));

            var response = await _client.PostAsync("/Sector/New", content);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void DeleteTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.DeleteAsync("/Sector/Delete?SectorId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void EditTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.PutAsync("/Sector/Edit?SectorId=1&content=", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Sector/Get?SectorId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllPersonalTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Sector/GetAllPersonal");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllPersonalByTypeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Sector/GetAllPersonalByType?TypeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllForEmployeeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Sector/GetAllForEmployee?EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllForEmployeeByTypeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Sector/GetAllForEmployeeByType?EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16&TypeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void AddSectorToResumeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.PostAsync("/Sector/AddToResume?ResumeId=1&content=test&TypeId=1", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void EditResumeSectorTest()
        {
            Assert.True(false);
        }
    }
}