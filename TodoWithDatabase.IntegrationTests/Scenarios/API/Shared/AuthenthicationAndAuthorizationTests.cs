using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Shared
{
    [Collection("BaseCollection")]
    public class AuthenthicationAndAuthorizationTests //: IClassFixture<TestContext> TODO research if this should be in a collection or not
    {
        private readonly TestContext _testContext;
        private readonly string _correctTokenForUser;
        private readonly string _incorrectToken;
        private readonly HttpContent _request;

        public AuthenthicationAndAuthorizationTests(TestContext testContext)
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

            var response = await SendRequest(urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/users", "POST")]
        [InlineData("/api/users/all", "GET")]
        public async Task Post_IncorrectToken_Forbidden(params string[] urlAndAction)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _incorrectToken);

            var response = await SendRequest(urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/users", "POST")]
        [InlineData("/api/users/all", "GET")]
        public async Task Post_MissingToken_Unauthorized(params string[] urlAndAction)
        {
            var response = await SendRequest(urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        private async Task<HttpResponseMessage> SendRequest(string[] urlAndAction)
        {
            var response = new HttpResponseMessage();
            switch (urlAndAction[1])
            {
                case "POST":
                    response = await _testContext.Client.PostAsync(urlAndAction[0], _request);
                    break;
                default:
                    response = await _testContext.Client.GetAsync(urlAndAction[0]);
                    break;
            }
            return response;
        }
    }
}
