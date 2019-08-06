using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TodoWithDatabase.Models;

namespace TodoWithDatabase.Services.Extensions
{
    public static class RoleCreator
    {
        public static async Task CreateRoles(this IServiceCollection services)
        {
            var roleManager = (RoleManager<IdentityRole>)services.BuildServiceProvider().GetService(typeof(RoleManager<IdentityRole>));

            var role1 = new IdentityRole {  };
            role1.Name = "TodoAdmin";
            if (!roleManager.RoleExistsAsync("TodoAdmin").Result)
            {
                IdentityResult var = await roleManager.CreateAsync(role1);
            };


            var role2 = new IdentityRole {  };
            if (!roleManager.RoleExistsAsync("TodoUser").Result)
            {
                await roleManager.CreateAsync(role2);
            };
        }
    }
}
