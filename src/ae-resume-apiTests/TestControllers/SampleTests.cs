using ae_resume_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ae_resume_api.Tests;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;
using System.IO;

namespace ae_resume_api.Controllers.Tests
{

    public class APITest : IClassFixture<WebApplicationFactory<ae_resume_api.Startup>>
    {
        protected readonly IConfigurationRoot _config;
        protected readonly HttpClient _client;
        protected readonly ApiTokenInMemoryClient _tokenService;

        public APITest(WebApplicationFactory<ae_resume_api.Startup> application)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            _client = application.CreateClient(new WebApplicationFactoryClientOptions()
                {
                    BaseAddress = new Uri(_config["API"])
                });

            _tokenService = new ApiTokenInMemoryClient(_config);
        }

    }
}
