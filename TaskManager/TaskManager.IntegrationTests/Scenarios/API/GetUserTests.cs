using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManager.IntegrationTests.Fixtures;
using TaskManager.IntegrationTests.Fixtures.Context;
using TaskManager.Models.DTOs;
using TaskManager.Services;
using Xunit;

namespace TaskManager.IntegrationTests.Scenarios.API
{
    [Collection("UserCollection")]
    public class GetUserTests
    {
        private readonly TestContext _testContext;
        private readonly string _user1Id;

        private readonly string _adminToken;
        private readonly string _user1Token;

        public GetUserTests(TestContext testContext)
        {
            _testContext = testContext;
            var tokenService = new TokenService();
            _user1Id = _testContext.Context.Assignees.First(a => a.UserName.Equals("user1Name")).Id;

            _adminToken = tokenService.GenerateToken("testId", "testAdmin", "TodoAdmin");
            _user1Token = tokenService.GenerateToken(_user1Id, "testAdmin", "TodoUser");
        }

        [Theory]
        [InlineData("/api/users/", "/userWithProjects")]
        [InlineData("/api/users/", "/userWithCards")]
        public async Task Get_CorrectId_AdminToken_ReturnsCorrectUserWithProjects(params string[] urlParts)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await _testContext.Client.GetAsync(urlParts[0] + _user1Id + urlParts[1]);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());

            if (urlParts[1].Contains("Project"))
            {
                Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithProjectsDTO)));
                Assert.Equal(_user1Id, JsonConvert.DeserializeObject<AssigneeWithProjectsDTO>(responseString).Id);
            }
            else
            {
                Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithCardsDTO)));
                Assert.Equal(_user1Id, JsonConvert.DeserializeObject<AssigneeWithCardsDTO>(responseString).Id);
            }
        }

        [Theory]
        [InlineData("/api/users/me/userWithProjects")]
        [InlineData("/api/users/me/userWithCards")]
        public async Task Get_UserToken_ReturnsCorrectUserWithProjects(string url)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _user1Token);

            var response = await _testContext.Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());

            if (url.Contains("Project"))
            {
                Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithProjectsDTO)));
                Assert.Equal(_user1Id, JsonConvert.DeserializeObject<AssigneeWithProjectsDTO>(responseString).Id);
            }
            else
            {
                Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithCardsDTO)));
                Assert.Equal(_user1Id, JsonConvert.DeserializeObject<AssigneeWithCardsDTO>(responseString).Id);
            }
        }
    }
}