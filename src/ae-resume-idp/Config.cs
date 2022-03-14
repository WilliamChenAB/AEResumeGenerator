using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace IdentityServer
{
    public class Config
    {
        IConfiguration _config;

        public Config(IConfiguration config)
        {
            _config = config;
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResource("roles", new List<string> { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("ae-resume-api")
            };

        //public static IEnumerable<ApiResource> ApiResources =>
        //    new ApiResource[]
        //    {
        //        new ApiResource("ae-resume-api-resource")
        //            {
        //                Scopes = { "ae-resume-api" },
        //                UserClaims = { "role" }
        //            }
        //    };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // JavaScript Client without backend
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris =           { "https://localhost:5002/callback.html" },
                    PostLogoutRedirectUris = { "https://localhost:5002/index.html" },
                    AllowedCorsOrigins =     { "https://localhost:5002" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ae-resume-api",
                        "roles",
                    }
                }
            };

    }
}