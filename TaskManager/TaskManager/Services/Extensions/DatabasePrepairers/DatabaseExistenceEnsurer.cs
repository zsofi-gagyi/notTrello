using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Repository;

namespace TaskManager.Services.Extensions.DatabaseSeeders
{
    public static class DatabaseExistenceEnsurer
    {
        public static void EnsureDatabaseIsCreated(this IServiceCollection services)
        {
            var context = services.BuildServiceProvider().GetRequiredService<MyContext>();
            context.Database.EnsureCreated();
        }
    }
}
