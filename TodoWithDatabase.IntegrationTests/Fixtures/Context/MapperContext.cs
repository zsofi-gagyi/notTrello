using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TodoWithDatabase.Repository;
using TodoWithDatabase.IntegrationTests.Helpers;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using TodoWithDatabase.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TodoWithDatabase.IntegrationTests
{
    public class MapperContext : IDisposable
    {
        private TestServer _server;

        public IMapper Mapper { get; set; }

        public MapperContext()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            _server = new TestServer(builder);

            Mapper = _server.Host.Services.GetRequiredService<IMapper>();
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}