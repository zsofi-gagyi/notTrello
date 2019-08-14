using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TodoWithDatabase.Services.Extensions
{
    public static class RoleCreator
    {
        public static async Task CreateRoles(this IServiceCollection services)
        {
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

            var role1 = new IdentityRole { Name = "TodoAdmin" };
            if (!roleManager.RoleExistsAsync("TodoAdmin").Result)
            {
                await roleManager.CreateAsync(role1);
            };

            var role2 = new IdentityRole { Name = "TodoUser" };
            if (!roleManager.RoleExistsAsync("TodoUser").Result)
            {
                await roleManager.CreateAsync(role2);
            };
        }
    }
}
