// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer
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
                    SubjectId = "1",
                    Username = "admin",
                    Password = "om!v!dzK#C3$$*W$OVNLofR6VxV1Ee%d",
                    Claims = {}
                }
            };
            }
        }
    }
}