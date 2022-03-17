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
    
    public class AttributesControllerTests: IClassFixture<WebApplicationFactory<ae_resume_api.Startup>>
    {
        readonly HttpClient _client;
        public AttributesControllerTests(WebApplicationFactory<ae_resume_api.Startup> application)
        {
            _client = application.CreateClient();
            Console.WriteLine(_client.BaseAddress);
        }

        [Fact]
        public void NewWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void CopyResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void DeleteWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetResumesForWorkspaceTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void CreateTemplateRequestTest()
        {
            Assert.True(false);
        }
    }
}