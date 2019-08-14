using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;
using TodoWithDatabase.Services.Interfaces;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace TodoWithDatabase.Services.Extensions.Middleware
{
    public class ProjectIdAccessVerifier
    {
        private readonly RequestDelegate _next;

        public ProjectIdAccessVerifier(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IProjectService projectService)
        {
            var path = context.Request.Path.Value;
            string[] urlParts = path.Split("/");
            var projectsIndex = urlParts.IndexOf("projects");
            var projectId = urlParts[projectsIndex + 1];

            string name = context.User.Claims.Where(c => c.Type.Equals("unique_name")).First().Value;
            var isAllowed = projectService.userCollaboratesOnProject(name, projectId); 

            if (isAllowed)
            {
                await _next(context);
            }
            else
            {
                context.Response.Clear();
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var responseObject = new { error = "User is not authorized to access this project" };
                var responseContent = JsonConvert.SerializeObject(responseObject);

                await context.Response.WriteAsync(responseContent);
            }
            return;
        }
    }
}
