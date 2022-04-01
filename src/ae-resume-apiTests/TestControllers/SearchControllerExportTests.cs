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
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System;

namespace ae_resume_api.Controllers.Tests
{    
    public class SearchControllerExportTests: APITest
    {

        public SearchControllerExportTests(WebApplicationFactory<Startup> application) : base(application)
        {
        }

        [Fact]
        public void SearchEmployeeResumesTestSearchEmployeeResumes()
        {
            Assert.True(false);
        }

        [Fact]
        public void SearchControllerTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SearchResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SearchSectorsTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SearchEmployeesTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task SearchEmployeeResumesTestAsync()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Search/AllEmployeeResumes?filter=Exported_ABC&EmployeeId=695bb64c-3c87-416c-8862-3bf4f5141f16");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void SearchEmployeeSectorsTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SearchEmployeeSectorsTest1()
        {
            Assert.True(false);
        }
    }
}