using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using IdentityModel;

namespace aeresumeidp
{
    public class Config
    {

        public Config()
        {
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("ae-resume-api")
            };

        //public static IEnumerable<ApiResource> ApiResources =>
        //    new ApiResource[]
        //    {
        //        new ApiResource("ae-resume-api")
        //    };

        public static IEnumerable<Client> Clients(IConfiguration config)
        {

            return new Client[]
            {
                // Test Client
                new Client()
                {
                    ClientId = "tests",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = true,
                    ClientSecrets = { new Secret(config.GetValue<string>("TestSecret").Sha256()) },

                    //AlwaysSendClientClaims = true,
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    //UpdateAccessTokenClaimsOnRefresh = true,
                    //AllowAccessTokensViaBrowser = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ae-resume-api"
                    }

                },

                // JavaScript Client without backend
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    //AlwaysSendClientClaims = true,
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    //UpdateAccessTokenClaimsOnRefresh = true,
                    //AllowAccessTokensViaBrowser = true,

                    RedirectUris =           { config.GetValue<string>("ApplicationCallback"),  "http://localhost:3000/auth/login-callback" },
                    PostLogoutRedirectUris = { config.GetValue<string>("ApplicationRedirect"), "http://localhost:3000/login"},
                    AllowedCorsOrigins =     { config.GetValue<string>("ApplicationURL"), "http://localhost:3000" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ae-resume-api"
                    }
                }
            };
        }
    }
}