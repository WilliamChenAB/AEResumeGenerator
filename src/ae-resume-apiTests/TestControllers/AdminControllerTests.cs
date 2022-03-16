using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace ae_resume_api.Controllers.Tests
{
    [TestClass()]
    public class AdminControllerTests: IClassFixture<WebApplicationFactory<ae_resume_api.Startup>>
    {
        readonly HttpClient _client;
        public AdminControllerTests(WebApplicationFactory<ae_resume_api.Startup> application)
        {            
            _client = application.CreateClient();
            Console.WriteLine(_client.BaseAddress);
        }

        [TestMethod()]
        public void NewEmployeeTest()
        {
            
        }

        [TestMethod()]
        public void EditEmployeeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteEmployeeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetEmployeeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public async void GetAllEmployeesTest()
        {
            var response = await _client.GetAsync("/Admin/GetAllEmployees");
            Assert.Equals(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod()]
        public void AssignAccessTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NewSectorTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditSectorTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSectorTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteSectorTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTemplateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTemplateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTempaltesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSectorsInTemplateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTemplateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AssignSectorTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTemplateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EmployeeEntityToModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TemplateEntityToModelTest()
        {
            Assert.Fail();
        }
    }
}