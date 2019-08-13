using System.Net.Http;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests;
using TodoWithDatabase.IntegrationTests.Fixtures;
using Xunit;

namespace TodoWithDatabase.TodoWithDatabase.IntegrationTests.Scenarios.API.Shared
{
    [Collection("BaseCollection")]
    public class AutenticationTests
    {
        private readonly TestContext _testContext;
        private readonly HttpContent _request;

        public AutenticationTests(TestContext testContext)
        {
            _testContext = testContext;
            _request = new StringContent("testRequest");
        }

        [Theory]
        [InlineData("/api/users", "POST")]
        [InlineData("/api/users/all", "GET")]
        public async Task Post_MissingToken_Unauthorized(params string[] urlAndAction)
        {
            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
