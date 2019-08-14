using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Fixtures;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.Shared
{
    [Collection("UnauthorizedCollection")]
    public class AutenticationTests
    {
        private readonly TestContext _testContext;
        private readonly HttpContent _request;
        private static string _userId;

        public AutenticationTests(TestContext testContext)
        {
            _testContext = testContext;
            _request = new StringContent("testRequest");
            _userId = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault().Id;
        }

        [Theory, MemberData(nameof(AuthenticatedEndpoints))]
        public async Task Post_MissingToken_Unauthorized(params string[] urlAndAction)
        {
            urlAndAction = InsertUserIdIfNecessary(urlAndAction);

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.False(response.IsSuccessStatusCode);
        }

        public static IEnumerable<object[]> AuthenticatedEndpoints
        {
            get
            {
                return new[]
                {
                    new string[] { "/api/users", "POST" },
                    new string[] { "/api/users/all", "GET" },
                    new string[] { "/api/users/me/userWithProjects", "GET" },
                    new string[] { "/api/users/me/userWithCards", "GET" },
                    new string[] { "/api/users/", "/userWithProjects", "GET" },
                    new string[] { "/api/users/",  "/userWithCards", "GET" }
                };
            }
        }

        private string[] InsertUserIdIfNecessary (string[] urlAndAction)
        {
            if (urlAndAction.Count() == 3)
            {
                urlAndAction = new string[] { urlAndAction[0] + _userId + urlAndAction[1], urlAndAction[2] };
            };

            return urlAndAction;
        }
    }
}
