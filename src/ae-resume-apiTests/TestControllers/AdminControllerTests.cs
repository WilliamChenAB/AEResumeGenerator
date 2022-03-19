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

namespace ae_resume_api.Controllers.Tests
{
    
    public class AdminControllerTests: IClassFixture<WebApplicationFactory<ae_resume_api.Startup>>
    {
        readonly HttpClient _client;
        public AdminControllerTests(WebApplicationFactory<ae_resume_api.Startup> application)
        {            
            _client = application.CreateClient();
            
            
           
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
            var response = await _client.GetAsync("/Admin/GetAllEmployees");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
            //var terms = JsonSerializer.Deserialize<List<GlossaryItem>>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Assert.Equal(3, terms.Count);
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
        public void CreateTemplateTest()
        {
           Assert.True(false);
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