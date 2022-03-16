using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ae_resume_apiTests.WebApplicationFactory
{
    public class WebApplicationFactory: WebApplicationFactory<ae_resume_api.Startup>
    {
        //TODO: I think we need to implement this override to properly use auth and the db connection
    }
}
