using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Fixtures;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.Shared
{
    [Collection("BaseCollection")]
    public class AutenticationTests
    {
        private readonly TestContext _testContext;
        private readonly HttpContent _request;
        private static string _user1Id;
        private static string _projectToDeleteId;

        public AutenticationTests(TestContext testContext)
        {
            _testContext = testContext;
            _request = new StringContent("testRequest");
            _user1Id = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault().Id;
            _projectToDeleteId = _testContext.Context.Projects.Where(p => p.Title.Equals("projectToDelete")).First().Id.ToString();
        }

        [Theory, MemberData(nameof(AuthenticatedEndpoints))]
        public async Task Post_MissingToken_Unauthorized(params string[] urlAndAction)
        {
            urlAndAction = InsertIdIfNecessary(urlAndAction);

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);

            Assert.Equal(HttpStatusCode.Unauthorized.ToString(), response.StatusCode.ToString());
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
                    new string[] { "/api/users/",  "/userWithCards", "GET" },
                    new string[] { "/api/users/me/projects/", "DELETE" }
                };
            }
        }

        private string[] InsertIdIfNecessary (string[] urlAndAction)
        {
            if (urlAndAction.Count() == 3)
            {
                urlAndAction = new string[] { urlAndAction[0] + _user1Id + urlAndAction[1], urlAndAction[2] };
            };

            if (urlAndAction[0].Contains("/projects"))
            {
                urlAndAction = new string[] { urlAndAction[0] + _projectToDeleteId, urlAndAction[1] };
            };

            return urlAndAction;
        }
    }
}
