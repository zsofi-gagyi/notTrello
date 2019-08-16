using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.Repository;
using TodoWithDatabase.IntegrationTests.Helpers;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using TodoWithDatabase.Services.Interfaces;
using AutoMapper;
using TaskManager;

namespace TodoWithDatabase.IntegrationTests
{
    public class AlwaysAuthenticatedTestContext : IDisposable
    {
        private TestServer _server;

        public HttpClient Client { get; set; }

        public MyContext Context { get; set; }

        public IMapper Mapper { get; set; }

        public AlwaysAuthenticatedTestContext()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("TestingWithoutAuthentication")
                .UseStartup<Startup>();

            _server = new TestServer(builder);
            Client = _server.CreateClient();

            Mapper = _server.Host.Services.GetRequiredService<IMapper>();

            Context = _server.Host.Services.GetRequiredService<MyContext>();

            var assigneeService = _server.Host.Services.GetRequiredService<IAssigneeService>();
            var projectService = _server.Host.Services.GetRequiredService<IProjectService>();
            DatabaseSeeder.InitializeDatabaseForHtmlTestsUsing(Context, assigneeService, projectService);
        }

        public void Dispose()
        {
            _server.Dispose();
            Client.Dispose();
        }
    }
}
