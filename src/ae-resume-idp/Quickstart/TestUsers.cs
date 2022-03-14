// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "admin",
                        Password = "admin",
                        Claims =
                        {
                            new Claim("role", "admin")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "464525",
                        Username = "employee",
                        Password = "employee",
                        Claims =
                        {
                            new Claim("role", "employee")
                        }
                    }
                };
            }
        }
    }
}