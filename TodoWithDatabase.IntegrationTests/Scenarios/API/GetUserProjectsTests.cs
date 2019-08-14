using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.App.TodoWithDatabase.IntegrationTests.Fixtures;
using TodoWithDatabase.Models.DTO;
using TodoWithDatabase.Services;
using Xunit;


namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Users
{
    [Collection("UserCollection")]
    public class GetUserProjectsTests
    {
        private readonly TestContext _testContext;
        private readonly string _user1Id;

        private readonly string _adminToken;
        private readonly string _user1Token;

        public GetUserProjectsTests(TestContext testContext)
        {
            _testContext = testContext;
            var tokenService = new TokenService();
            _user1Id = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault().Id;

            _adminToken = tokenService.GenerateToken("testId", "testAdmin", "TodoAdmin");
            _user1Token = tokenService.GenerateToken(_user1Id, "testAdmin", "TodoUser");
        }

        [Theory]
        [InlineData("/api/users/", "/userWithProjects")]
        public async Task Get_CorrectId_AdminToken_ReturnsCorrectUserWithProjects(params string[] urlParts)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await _testContext.Client.GetAsync(urlParts[0] + _user1Id + urlParts[1]);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
            Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithProjectsDTO)));
            Assert.Equal(_user1Id, JsonConvert.DeserializeObject<AssigneeWithProjectsDTO>(responseString).Id);
        }

        [Theory]
        [InlineData("/api/users/me/userWithProjects")]
        public async Task Get_UserToken_ReturnsCorrectUserWithProjects(string url)
        {
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _user1Token);

            var response = await _testContext.Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
            Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithProjectsDTO)));
            Assert.Equal(_user1Id, JsonConvert.DeserializeObject<AssigneeWithProjectsDTO>(responseString).Id);
        }
    }
}