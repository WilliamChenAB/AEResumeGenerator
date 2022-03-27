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


    public class FacadeControllerTests : APITest
    {

        public FacadeControllerTests(WebApplicationFactory<Startup> application) : base(application)
        {
        }

        [Fact]
        public void NewResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void NewSectorTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void GetSectorTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);
            
            var response = await _client.GetAsync("/Facade/GetSector?SectorId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void GetAllSectorsForEmployeeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetAllSectorsForEmployeeByTypeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void DeleteResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void DeleteSectorTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void EditSectorTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var stringContent = new StringContent("");
            var response = await _client.PutAsync("/Facade/EditSector?SectorId=1&content=some%20content&division=Utility&Image=test.png", stringContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void AddSectorToResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void EditResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void GetResumesForEmployeeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Facade/GetResumesForEmployee?EmployeeId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void GetPersonalResumesForEmployeeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void ExportResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void ExportResumesInWorkspaceXML()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var request = new HttpRequestMessage(HttpMethod.Get, "/Facade/ExportResumesInWorkspaceXML.xml?WorkspaceId=4");
            request.Headers.Add("accept", "application/xml");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
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
        public async void SearchEmployeesTest()
        {
            
                var token = await _tokenService.GetSAAccessToken();
                _client.SetBearerToken(token);

                var response = await _client.GetAsync("/Facade/SearchEmployees?filter=james");
                response.EnsureSuccessStatusCode();
                var stringResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(stringResponse);
            
        }

        [Fact]
        public async void SearchEmployeeResumeTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Facade/SearchAllEmployeeResumes?filter=test&EmployeeId=eebc735b-b277-43f0-a882-8391177dd93a");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void SearchWorkspacesTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void GetAllTemplatesTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Facade/GetAllTemplates");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }

        [Fact]
        public void SectorEntityToModelTest()
        {
            Assert.True(false);
        }

        [Fact]
        public async void ExportResumesInWorkspaceTest()
        {
            var token = await _tokenService.GetSAAccessToken();
            _client.SetBearerToken(token);

            var response = await _client.GetAsync("/Facade/ExportResumesInWorkspace?WorkspaceId=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(stringResponse);
        }
    }
}