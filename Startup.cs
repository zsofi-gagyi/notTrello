using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.StaticFiles;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TodoWithDatabase.Services.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Linq;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.Services.Extensions.Middleware;
using TodoWithDatabase.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace TodoWithDatabase
{
    public class Startup
    {
        private static void UseIdVerifier(IApplicationBuilder app) // middleware-related stuff
        {
            app.UseMiddleware<IdVerifier>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>(options => options.UseMySql("server=localhost;database=todosincsharp;user=root;password=000password000",
                  mySqlOptions =>
                  {
                      mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql);
                  }
                 ));

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
                         ["error"] = "Unauthorized - user not recognized as an assignee"
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
                options.LoginPath = new PathString("/login.html");
            });

            services.AddTransient<IAssigneeService, AssigneeService>();
            services.AddTransient<ICardService, CardService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.setUpAutoMapper();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseWhen(context => context.Request.Path.ToString().Contains("projects"), UseIdVerifier);

            app.UseCookiePolicy();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
