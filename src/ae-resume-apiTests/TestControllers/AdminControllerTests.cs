using ae_resume_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ae_resume_api.DBContext;
using Microsoft.AspNetCore.Mvc.Testing;
using ae_resume_api.Admin;
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
    
    public class AdminControllerTests: IClassFixture<WebApplicationFactory<ae_resume_api.Startup>>
    {
        private readonly IConfigurationRoot _config;
        private readonly HttpClient _client;
        private readonly ApiTokenInMemoryClient _tokenService;
        public AdminControllerTests(WebApplicationFactory<ae_resume_api.Startup> application)
        {            
            _client = application.CreateClient();

            _config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.Development.json", optional: true)
               .Build();

            _client = application.CreateClient(new WebApplicationFactoryClientOptions()
            {
                BaseAddress = new Uri(_config["Tests:API"])
            });

            _tokenService = new ApiTokenInMemoryClient(_config);


        }

        [Fact]
        public void NewEmployeeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void EditEmployeeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void DeleteEmployeeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetEmployeeTest()
        {
           Assert.True(false);
        }

        [Fact]
        public async Task GetAllEmployeesTest()
        {

            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Admin/GetAllEmployees");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void AssignAccessTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void NewSectorTypeTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void EditSectorTypeTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void GetSectorTypeTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void DeleteSectorTypeTest()
        {
           Assert.True(false);
        }

        [Fact]
        public async void CreateTemplateTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            TemplateModel template = new TemplateModel
            {
                Title = "Create template test",
                Description = "test template for api tests",                
                SectorTypes = new List<SectorTypeModel> { 
                    new SectorTypeModel { TypeID = 1 },
                    new SectorTypeModel { TypeID = 2 },
                    new SectorTypeModel { TypeID = 3 }
                }
            };
            var response = await _client.PostAsJsonAsync("/Admin/CreateTemplate", template);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void GetTemplateTest()
        {
           Assert.True(false);
        }        

        [Fact]
        public void GetSectorsInTemplateTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void EditTemplateTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void AssignSectorTypeTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void DeleteTemplateTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void EmployeeEntityToModelTest()
        {
           Assert.True(false);
        }

        [Fact]
        public void TemplateEntityToModelTest()
        {
           Assert.True(false);
        }
    }
}