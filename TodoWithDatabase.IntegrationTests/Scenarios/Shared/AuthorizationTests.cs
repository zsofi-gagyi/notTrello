using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Fixtures;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Shared
{
    [Collection("BaseCollection")]
    public class AuthorizationTests 
    {
        private readonly TestContext _testContext;
        private static string _userId;
        private readonly string _correctTokenForUser;
        private readonly string _incorrectToken;
        private readonly HttpContent _request;


        public AuthorizationTests(TestContext testContext)
        {
            _testContext = testContext;
            _userId = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault().Id;

            var tokenService = new TokenService();
            _correctTokenForUser = tokenService.GenerateToken(_userId, "testUser", "TodoUser"); 
            _incorrectToken = tokenService.GenerateToken(_userId, "testUser", "IncorrectRole");

            _request = new StringContent("testRequest");
  
        }

        [Theory, MemberData(nameof(RestrictedToAdminEndpoints))]
        public async Task Post_TokenForUser_Forbidden(params string[] urlAndAction)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _correctTokenForUser);

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory, MemberData(nameof(RestrictedToAdminEndpoints))]
        public async Task Post_IncorrectToken_Forbidden(params string[] urlAndAction)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _incorrectToken);

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        public static IEnumerable<object[]> RestrictedToAdminEndpoints
        {
            get
            {
                return new[]
                {
                    new string[] { "/api/users", "POST" },
                    new string[] { "/api/users/all", "GET" },
                    new string[] { "/api/users/" + _userId + "/userWithProjects", "GET" }
                };
            }
        }
    }
}
