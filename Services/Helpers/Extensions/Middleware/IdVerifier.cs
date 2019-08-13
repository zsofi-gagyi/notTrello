using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;
using TodoWithDatabase.Services.Interfaces;
using System;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace TodoWithDatabase.Services.Extensions.Middleware
{
    public class IdVerifier
    {
        private readonly RequestDelegate _next;

        public IdVerifier(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IProjectService projectService)
        {
            var path = context.Request.Path.Value;
            string[] urlParts = path.Split("/");
            var projectsIndex = urlParts.IndexOf("projects");
            var projectId = urlParts[projectsIndex + 1];

            string name = context.User.Identity.Name;
            var isAllowed = projectService.userCollaboratesOnProject(name, projectId); 

            if (isAllowed)
            {
                await _next(context);
            }
            else
            {
                context.Response.Redirect("/users");
            }
        }
    }
}
