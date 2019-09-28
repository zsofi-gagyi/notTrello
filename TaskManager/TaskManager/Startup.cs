using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using TaskManager.Repository;
using TaskManager.Services;
using TaskManager.Services.Extensions;
using TaskManager.Services.Extensions.Middleware;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DAOs;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TaskManager.Services.Extensions.DatabaseSeeders;

namespace TaskManager
{
    public class Startup
    {
        private IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<MyContext>
                (options => options.UseMySql
                    (   $"server=   {Environment.GetEnvironmentVariable("TaskManagerHOST")};" +
                        $"database= {Environment.GetEnvironmentVariable("TaskManagerDATABASE")};" +
                        $"user=     {Environment.GetEnvironmentVariable("TaskManagerUSERNAME")};" +
                        $"password= {Environment.GetEnvironmentVariable("TaskManagerPASSWORD")};",
                        mySqlOptions =>
                        {
                            mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
                        }
                    )
                , ServiceLifetime.Scoped);

            services.EnsureDatabaseIsCreated();

            AddEnvironmentNeutralConfigurations(services);
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/users/changeRole");
            });

            services.EnsureDatabaseHasStandardGuestData();
        }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>(builder => builder.UseInMemoryDatabase("InMemory"), ServiceLifetime.Singleton);

            AddEnvironmentNeutralConfigurations(services);
        }

        public void ConfigureTestingWithoutAuthenticationServices(IServiceCollection services)
        {
            ConfigureTestingServices(services);
            services.AddMvc(options =>
            {
                options.Filters.Add(new AllowAnonymousFilter());
            });
        }

        public void AddEnvironmentNeutralConfigurations(IServiceCollection services)
        {
            services.AddIdentity<Assignee, IdentityRole>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["ClientIdTaskManager"] ?? "testingId";
                    options.ClientSecret = googleAuthNSection["ClientSecretTaskManager"] ?? "testingSecret"; ;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TODOTOKENSECRET"))),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };

                    config.Events = new JwtBearerEvents();
                    config.Events.OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;

                        var payload = new JObject
                        {
                            ["error"] = "Unauthorized - user not recognized"
                        };

                        return context.Response.WriteAsync(payload.ToString());
                    };
                }
           );

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/";
                options.Cookie.Name = "TodoCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.ReturnUrlParameter = "returnTo";
                options.SlidingExpiration = true;
                options.LoginPath = new PathString("/login");
                options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
                options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
            });

            services.AddScoped<IAssigneeService, AssigneeService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProjectService, ProjectService>();

            services.SetUpAutoMapper();

            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/users/changeRole");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseWhen(context => (context.Request.Path.ToString().Contains("userWithProjects") ||
                                    context.Request.Path.ToString().Contains("userWithCards")) &&
                                   !context.Request.Path.ToString().Contains("me"),
                        UseUserIdExistenceVerifier);

            app.UseCookiePolicy();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

        private static void UseUserIdExistenceVerifier(IApplicationBuilder app)
        {
            app.UseMiddleware<UserIdExistenceVerifier>();
        }

        private static Func<RedirectContext<CookieAuthenticationOptions>, Task> 
            ReplaceRedirector(HttpStatusCode statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
                context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = (int)statusCode;
                        return Task.CompletedTask;
                    }

                    return existingRedirector(context);
                };
    }
}