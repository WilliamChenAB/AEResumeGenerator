using ae_resume_api.DBContext;
using Microsoft.EntityFrameworkCore;
using ae_resume_api.Authorization;
using Microsoft.AspNetCore.Authorization;
using ae_resume_api.Controllers;

namespace ae_resume_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // For Entity Framework
            services.AddScoped(x => {
                DbContextOptionsBuilder<DatabaseContext> dbBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                dbBuilder.UseLazyLoadingProxies()
                         .UseSqlServer(Configuration.GetConnectionString("ConnStr11"));
                return new DatabaseContext(dbBuilder.Options);
            });

            services.AddScoped<IAuthorizationHandler, AccessHandler>();

            // Adding Authentication
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["Authority"];
                    //options.Audience = Configuration["API"];
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    // it's recommended to check the type header to avoid "JWT confusion" attacks
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "ae-resume-api");
                    policy.Requirements.Add(new AccessRequirement(Access.Employee));
                });
                options.AddPolicy("PA",
                    policy => policy.Requirements.Add(new AccessRequirement(Access.ProjectAdmin))
                );
                options.AddPolicy("SA",
                    policy => policy.Requirements.Add(new AccessRequirement(Access.SystemAdmin))
                );
            });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(Configuration["AllowedCORS"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMvc()
                    .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IConfiguration config, DatabaseContext context)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("default");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });

            if (!context.Employee.Any())
            {
                var admin = new AdminController(context, config);
                var task2 = admin.LoadTestData();
                task2.Wait();
                var task = admin.LoadDefaultAdmin();
                task.Wait();
            }

        }
    }
}
