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
            new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("ae-resume-api")
            };

        public IEnumerable<Client> Clients() {
            string clientString =_config.GetValue<string>("ApplicationURL");

            return new Client[]
            {
                // JavaScript Client without backend
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris =           { clientString + "/callback.html" },
                    PostLogoutRedirectUris = { clientString + "/index.html" },
                    AllowedCorsOrigins =     { clientString },

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