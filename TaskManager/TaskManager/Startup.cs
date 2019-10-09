using System;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TaskManager.Services.Extensions.DatabaseSeeders;
using TaskManager.Repository;
using TaskManager.Services;
using TaskManager.Services.Extensions;
using TaskManager.Services.Extensions.Middleware;
using TaskManager.Services.Interfaces;
using TaskManager.Models.DAOs;

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
                (options => options.UseSqlServer("name=DefaultConnection"), ServiceLifetime.Scoped);

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
            services.AddDbContext<MyContext>
                (builder => builder.UseInMemoryDatabase("InMemory"), ServiceLifetime.Singleton);

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
                    options.ClientId = Environment.GetEnvironmentVariable("ClientIdTaskManager"); // stored in the Key Vault, accessed via Application Settings 
                    options.ClientSecret = Environment.GetEnvironmentVariable("ClientSecretTaskManager"); // stored in the Key Vault, accessed via Application Settings 
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenSecretTaskManager"))), // stored in the Key Vault, accessed via Application Settings 
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
                options.CookieHttpOnly = true;
                options.CookieSecure = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
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