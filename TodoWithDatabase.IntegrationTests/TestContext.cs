using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.Repository;
using TodoWithDatabase.IntegrationTests.Helpers;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.IntegrationTests
{
    public class TestContext : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; set; }
        public MyContext Context { get; set; }

        public TestContext()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            server = new TestServer(builder);
            Client = server.CreateClient();

            Context = server.Host.Services.GetRequiredService<MyContext>();
            var assigneeService = server.Host.Services.GetRequiredService<IAssigneeService>();
            DatabaseSeeder.InitializeDatabaseForTestsUsing(Context, assigneeService);
        }

        public void Dispose()
        {
            server.Dispose();
            Client.Dispose();
            Context.Dispose();
        }
    }
}
                    