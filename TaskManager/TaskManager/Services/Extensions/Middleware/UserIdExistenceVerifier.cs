using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using TodoWithDatabase.Repository;
using System.Linq;

namespace TodoWithDatabase.App.Services.Helpers.Extensions.Middleware
{
    public class UserIdExistenceVerifier
    {
        private readonly RequestDelegate _next;

        public UserIdExistenceVerifier(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            string[] urlParts = path.Split("/");

            var keywordIndex = path.Contains("userWithProjects") ? 
                urlParts.IndexOf("userWithProjects") : 
                urlParts.IndexOf("userWithCards");

            var userId = urlParts[keywordIndex - 1];

            var myContext = (MyContext)context.RequestServices.GetService(typeof (MyContext));

            var idExists = myContext.Users.Where(u => u.Id.ToString().Equals(userId)).FirstOrDefault() != null;

            if (idExists)
            {
                await _next(context);
            }
            else    // i'd prefer to get rid of this "else" clause, and put it all just after the IF has failed,
                    // but the threads somehow get mixed up otherwise and the test fails. (comment to be deleted)
            {

                context.Response.Clear();
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = "Incorrect user Id" }));
            }
            return;
        }
    }
}
