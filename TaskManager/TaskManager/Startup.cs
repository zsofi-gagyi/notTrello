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

using TodoWithDatabase.Repository;
using TodoWithDatabase.Services;
using TodoWithDatabase.Services.Extensions;
using TodoWithDatabase.App.Services.Helpers.Extensions.Middleware;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Models.DAOs;

namespace TaskManager
{
    public class Startup
    {
        private static void UseUserIdExistenceVerifier(IApplicationBuilder app)
        {
            app.UseMiddleware<UserIdExistenceVerifier>();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<MyContext>(options => options.UseMySql("server=localhost;database=todosincsharp;user=root;password=000password000", //TODO move these into environmental variables
                mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
                }
            ));

            AddEnvironmentNeutralConfigurations(services);

        }
        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>(builder => builder.UseInMemoryDatabase("InMemory"), ServiceLifetime.Singleton);

            AddEnvironmentNeutralConfigurations(services);
        }

        public void AddEnvironmentNeutralConfigurations(IServiceCollection services)
        {
            services.AddIdentity<Assignee, IdentityRole>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyContext>();

            services.AddAuthentication()
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
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/";
                options.Cookie.Name = "TodoCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.ReturnUrlParameter = "returnTo";
                options.SlidingExpiration = true;
                options.LoginPath = new PathString("/login");
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
    }
}