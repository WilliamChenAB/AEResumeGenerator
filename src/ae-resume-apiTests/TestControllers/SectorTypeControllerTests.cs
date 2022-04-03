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
    
    public class SectorTypeControllerTests : APITest
    {
        
        public SectorTypeControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public async void NewTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/SectorType/New?title=test&description=test");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void EditTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.PutAsync("/SectorType/Edit?SectorTypeId=1&title=test", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void EditTitleTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.PutAsync("/SectorType/EditTitle?SectorTypeId=1&title=test", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/SectorType/Get?SectorTypeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void DeleteTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.DeleteAsync("/SectorType/Delete?SectorTypeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public async void GetAllTest()
        {
            var token = await _tokenService.GetTestDataToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/SectorType/GetAll");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
    }
}