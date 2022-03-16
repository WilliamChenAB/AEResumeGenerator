using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using IdentityModel;

namespace aeresumeidp
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

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // JavaScript Client without backend
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireClientSecret = false,

                    //AlwaysSendClientClaims = true,
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =           { "https://localhost:5002/callback.html" },
                    PostLogoutRedirectUris = { "https://localhost:5002/index.html" },
                    AllowedCorsOrigins =     { "https://localhost:5002" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ae-resume-api"
                    }
                }
            };

    }
}