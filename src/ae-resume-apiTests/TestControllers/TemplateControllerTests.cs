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
    
    public class TemplateControllerTests : APITest
    {
        
        public TemplateControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public async void CreateTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            TemplateModel entity = new TemplateModel
            {
                Title = "test",
                Description = "test",
                SectorTypes = new List<SectorTypeModel>
                {
                    new SectorTypeModel{TypeId = 1},
                    new SectorTypeModel{TypeId = 2}
                }
            };

            var response = await _client.PostAsJsonAsync("/Template/Create?title=test&description=test", entity);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            
            var response = await _client.GetAsync("/Template/Get?TemplateId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void EditTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.PutAsync("/Template/Edit?TemplateId=1&title=new", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void DeleteTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.DeleteAsync("/Template/Delete?TemplateId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetSectorsTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Template/GetSectors?TemplateId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void AssignSectorTypeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void GetAllTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);


            var response = await _client.GetAsync("/Template/GetAll");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
    }
}