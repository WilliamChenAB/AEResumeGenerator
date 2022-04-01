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
    public class WorkspaceControllerTests : APITest
    {
        public WorkspaceControllerTests(WebApplicationFactory<ae_resume_api.Startup> application) : base(application)
        {
        }

        [Fact]
        public void NewTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void DeleteTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetPersonalTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetResumesTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void CreateTemplateRequestTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void AddEmptyResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void CopyResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void SubmitResumeTest()
        {
            Assert.True(false);
        }

        [Fact]
        public void GetAllSectorTypesTest()
        {
            Assert.True(false);
        }
    }
}