using ae_resume_api.DBContext;
using Microsoft.EntityFrameworkCore;
using ae_resume_api.Authorization;
using Microsoft.AspNetCore.Authorization;

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
            services.AddDbContext<DatabaseContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseSqlServer(Configuration.GetConnectionString("ConnStr"))
            );

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

            // adds an authorization policy to make sure the token is for scope 'api1'
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
