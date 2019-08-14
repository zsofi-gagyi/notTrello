using Xunit;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Helpers;
using System.Net.Http;
using TodoWithDatabase.Models.DTOs;
using Newtonsoft.Json;
using System.Net;

namespace TodoWithDatabase.IntegrationTests
{
    public class ToBeDeletedAfterRefactoring //: IClassFixture<WebApplicationFactory<Startup>>
    {
       /* private readonly WebApplicationFactory<Startup> _factory;

        public ToBeDeletedAfterRefactoring(WebApplicationFactory<Startup> factory)
        {
           _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/login")]
        [InlineData("/1")]
        [InlineData("/add/1")]
        public async Task Get_EndpointsReturn_SuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/")]
        public async Task Get_EndpointReturns_CorrectTitleInBody(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            var content = await HtmlParser.Parse(response);
            var titleElement = content.QuerySelectorAll(".title");
            var firstTitle = content.QuerySelector(".title");

            Assert.Equal(1, titleElement.Length);
            Assert.Equal("Todos for today:", firstTitle.TextContent);
        }

        [Theory]
        [InlineData("/")]
        public async Task Get_EndpointReturns_CorrenctNrOfTodos(string url)
        {
            var client = _factory.CreateClient();   //  -->  this will lead to it still using the real database, not the mock one ... why? 
                                                    //also the following solution doesn't work either

           /* var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<MyContext>();

                        try
                        {
                            Utilities.InitializeDbForTests(db);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine("database setup failed");
                        }
                    }
                });
            })
            .CreateClient();
            

            var response = await client.GetAsync(url);
            var content = await HtmlParser.Parse(response);
            var todoElement = content.QuerySelectorAll(".todo");

            Assert.Equal(9, todoElement.Length);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/1")]
        public async Task Get_EndpointReturns_CorrectFirstTodo(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            var content = await HtmlParser.Parse(response);
            var firstTodo = content.QuerySelector("tr.todo td.description");  // this gets the first one

            Assert.Equal("to sleep", firstTodo.TextContent);
        }

       */
    }
}