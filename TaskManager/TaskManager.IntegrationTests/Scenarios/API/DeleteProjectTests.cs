using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Helpers;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Users
{
    [Collection("DeleteUserCollection")]
    public class DeleteProjectTests
    {
        private readonly TestContext _testContext;
        private readonly TokenService _tokenService;
        private readonly string _soloProjectId;
        private readonly string _sharedProjectId;

        public DeleteProjectTests(TestContext testContext)
        {
            _testContext = testContext;
            _tokenService = new TokenService();

            var projectNeedsRegenerating = _testContext.Context.Projects.Where(p => p.Title.Equals("projectToDelete")).Count() == 0;
            if (projectNeedsRegenerating)                                          // this should not be a problem, I need to learn more about how test suites work (comment to be deleted)
            {
                var assignee1 = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault();
                _testContext.Context.CreateProjectToDeleteFor(assignee1);
            }

            _soloProjectId = _testContext.Context.Projects.Where(p => p.Title.Equals("projectToDelete")).First().Id.ToString();
            _sharedProjectId = _testContext.Context.Projects.Where(p => p.Title.Equals("sharedProject")).First().Id.ToString();
        }

        [Theory]
        [InlineData("/api/users/me/projects/")]
        public async Task Delete_UserIsAuthorized_Delete(string url)
        {
            AuthorizeClientAsUser1();

            var response = await _testContext.Client.DeleteAsync(url + _soloProjectId);

            Assert.Equal(HttpStatusCode.NoContent.ToString(), response.StatusCode.ToString());
            Assert.True(_testContext.Context.Projects.Where(p => p.Id.ToString().Equals(_soloProjectId)).Count() == 0);
        }

        [Theory]
        [InlineData("/api/users/me/projects/")]
        public async Task Delete_ProjectIsNotOfUsers_Forbidden(string url)
        {
            var expectedObject = new { message = "Users can delete only their own projects." };
            var expectedString = JsonConvert.SerializeObject(expectedObject);

            var user2 = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user2Name")).FirstOrDefault();
            var user2Token = _tokenService.GenerateToken(user2.Id, "user2Name", "TodoUser");
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user2Token);

            var response = await _testContext.Client.DeleteAsync(url + _soloProjectId);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Forbidden.ToString(), response.StatusCode.ToString());
            Assert.Equal(expectedString, responseString);
        }

        [Theory]
        [InlineData("/api/users/me/projects/")]
        public async Task Delete_ProjectIsShareds_BadRequest(string url) // this could be "forbidden" too, but i feel it's 
                                                                         // maybe clearer like this? (comment to be deleted)
        {
            var expectedObject = new { message = "The project is shared with other users. Only solo projects can be deleted using this method" };
            var expectedString = JsonConvert.SerializeObject(expectedObject);

            AuthorizeClientAsUser1();

            var response = await _testContext.Client.DeleteAsync(url + _sharedProjectId);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest.ToString(), response.StatusCode.ToString());
            Assert.Equal(expectedString, responseString);
        }

        private void AuthorizeClientAsUser1()
        {
            var user1 = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault();
            var user1Token = _tokenService.GenerateToken(user1.Id, "user1Name", "TodoUser");
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user1Token);
        }
    }
}