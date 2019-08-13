using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Fixtures;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Shared
{
    [Collection("BaseCollection")]
    public class AuthorizationTests //: IClassFixture<TestContext> TODO research if this should be in a collection or not
    {
        private readonly TestContext _testContext;
        private readonly string _correctTokenForUser;
        private readonly string _incorrectToken;
        private readonly HttpContent _request;

        public AuthorizationTests(TestContext testContext)
        {
            _testContext = testContext;

            var tokenService = new TokenService();
            _correctTokenForUser = tokenService.GenerateToken("testUser", "TodoUser", false);  // TODO this must be researched and changed to "true"
            _incorrectToken = tokenService.GenerateToken("testUser", "IncorrectRole", false);

            _request = new StringContent("testRequest");
        }

        [Theory]
        [InlineData("/api/users", "POST" )]
        [InlineData("/api/users/all", "GET")]
        public async Task Post_TokenForUser_Forbidden(params string[] urlAndAction)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _correctTokenForUser);

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/users", "POST")]
        [InlineData("/api/users/all", "GET")]
        public async Task Post_IncorrectToken_Forbidden(params string[] urlAndAction)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _incorrectToken);

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
