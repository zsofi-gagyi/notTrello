using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;
using TodoWithDatabase.Services.Interfaces;
using System;
using Newtonsoft.Json;

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
            var projectId = urlParts[projectsIndex - 1];

            string name = context.User.Identity.Name;
            var isAllowed = projectService.userCollaboratesOnProject(name, projectId); 

            if (isAllowed)
            {
                await _next(context);
            }

            context.Response.Clear();
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var responseObject = new { error = "User is not authorized to access this project" };
            var responseContent = JsonConvert.SerializeObject(responseObject);

            await context.Response.WriteAsync(responseContent);
            return;

        }
    }
}
