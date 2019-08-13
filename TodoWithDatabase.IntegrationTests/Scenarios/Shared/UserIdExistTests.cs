using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Fixtures;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.Shared
{
    [Collection("BaseCollection")]
    public class UserIdExistsTests
    {
        private readonly TestContext _testContext;
        private readonly HttpContent _request;

        public UserIdExistsTests(TestContext testContext)
        {
            _testContext = testContext;

            var tokenService = new TokenService();
            var correctToken = tokenService.GenerateToken("testUser", "TodoAdmin", false);
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", correctToken);
            _request = new StringContent("testRequest");
        }

        [Theory, MemberData(nameof (ProjectRelatedEndpoints))]
        public async Task InexistentProjectId_BadRequest(params string[] urlAndAction)
        {
            var expectedString = JsonConvert.SerializeObject(new { error = "Incorrect user Id" });

            var response = await RequestSender.Send(_testContext.Client, _request, urlAndAction);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest.ToString(), response.StatusCode.ToString());
            Assert.Equal(expectedString, responseString);
        }

        public static IEnumerable<object[]> ProjectRelatedEndpoints
        {
            get
            {
                return new[]
                {
                    new string[] { "/api/users/00000000-0000-0000-0000-000000000000/userWithProjects", "GET" }
                };
            }
        }
    }
}