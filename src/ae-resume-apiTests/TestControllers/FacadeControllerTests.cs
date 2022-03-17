using ae_resume_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ae_resume_api.Controllers.Tests
{
    
    public class FacadeControllerTests: IClassFixture<WebApplicationFactory<ae_resume_api.Startup>>
    {
        readonly HttpClient _client;
        public FacadeControllerTests(WebApplicationFactory<ae_resume_api.Startup> application)
        {
            _client = application.CreateClient();
            Console.WriteLine(_client.BaseAddress);
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
        public void GetSectorTest()
        {
            Assert.True(false);
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
        public void EditSectorTest()
        {
            Assert.True(false);
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
        public void GetResumesForEmployeeTest()
        {
            Assert.True(false);
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
        public void SearchEmployeeResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SearchWorkspacesTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SectorEntityToModelTest()
        {
            Assert.True(false);
        }
    }
}