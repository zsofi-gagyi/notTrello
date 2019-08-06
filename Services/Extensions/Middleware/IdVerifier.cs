using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWithDatabase.Services.Extensions.Middleware
{
    public class IdVerifier
    {
        private readonly RequestDelegate _next;

        public IdVerifier(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAssigneeService assigneeService)
        {
            var path = context.Request.Path.Value;
            string[] urlParts = path.Split("/");
            var usersIndex = urlParts.IndexOf("users");
            var id = urlParts[usersIndex + 1];

            if (id.Equals("x") || !assigneeService.Exists(id))
            {
                context.Response.Redirect("https://localhost:44343/login.html");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
