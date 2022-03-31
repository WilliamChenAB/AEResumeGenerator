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
    public class AdminControllerTests : APITest
    {

        public AdminControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
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
                    new SectorTypeModel { TypeId = 1 },
                    new SectorTypeModel { TypeId = 2 },
                    new SectorTypeModel { TypeId = 3 }
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
        public async void AssignSectorTypeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            List<int> ids = new List<int> { 1 };

            var response = await _client.PostAsJsonAsync("/Admin/AssignSectorType?templateID=10", ids);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
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

        [Fact]
        public async void EditSectorTypeTitleTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.PutAsync("/Admin/EditSectorTypeTitle?SectorTypeId=11&title=new", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
        [Fact]
        public async void TestDateTime()
        {
            string test = CurrentTimeAsString();

            string date = "";
            DateTime dt = parseDate(test);
        }

        private static readonly string DATE_TIME_FORMAT = "yyyyMMdd HH:mm:ss zzz";
        public static DateTime parseDate(string dateTime)
        {
            return DateTime.ParseExact(dateTime, DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        public static string CurrentTimeAsString()
        {           
            return DateTime.UtcNow.ToString(DATE_TIME_FORMAT);
        }
    }
}