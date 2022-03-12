using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer;
using IdentityServer.Pages.Admin.ApiScopes;
using IdentityServer.Pages.Admin.Clients;
using IdentityServer.Pages.Admin.IdentityScopes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
                })
                .AddTestUsers(TestUsers.Users)
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
                })

                // this is something you will want in production to reduce load on and requests to the DB
                //.AddConfigurationStoreCache()

                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.RemoveConsumedTokens = true;
                });

            // this adds the necessary config for the simple admin/config pages
            {
                builder.Services.AddAuthorization(options =>
                    options.AddPolicy("admin",
                        policy => policy.RequireClaim("sub", "1"))
                );

                builder.Services.Configure<RazorPagesOptions>(options =>
                    options.Conventions.AuthorizeFolder("/Admin", "admin"));

                builder.Services.AddTransient<ClientRepository>();
                builder.Services.AddTransient<IdentityScopeRepository>();
                builder.Services.AddTransient<ApiScopeRepository>();
            }

            return builder.Build();
        }

        private static void InitializeDatabase(IConfiguration configuration, IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    Config config = new Config(configuration);

                    foreach (var client in config.Clients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

        public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration config)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitializeDatabase(config, app);

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapRazorPages()
                .RequireAuthorization();

            return app;
        }
    }
}