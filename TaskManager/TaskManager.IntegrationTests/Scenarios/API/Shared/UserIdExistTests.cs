using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManager.IntegrationTests.Fixtures.Context;
using TaskManager.Services;
using Xunit;

namespace TaskManager.IntegrationTests.Scenarios.API.Shared
{
    [Collection("BaseCollection")]
    public class UserIdExistsTests
    {
        private readonly TestContext _testContext;

        public UserIdExistsTests(TestContext testContext)
        {
            _testContext = testContext;

            var tokenService = new TokenService();
            var correctToken = tokenService.GenerateToken("testId", "testUser", "TodoAdmin");
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", correctToken);
        }

        [Theory, MemberData(nameof (ProjectRelatedEndpoints))]
        public async Task InexistentProjectId_BadRequest(string url)
        {
            var expectedString = JsonConvert.SerializeObject(new { error = "Incorrect user Id" });

            var response = await _testContext.Client.GetAsync(url);
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
                    new string[] { "/api/users/00000000-0000-0000-0000-000000000000/userWithProjects" },
                    new string[] { "/api/users/00000000-0000-0000-0000-000000000000/userWithCards" } 
                };
            }
        }
    }
}