using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManager.TestUtilities.TestObjectMakers;
using TaskManager.IntegrationTests.Fixtures;
using TaskManager.Services;
using Xunit;
using TaskManager.IntegrationTests.Fixtures.Context;
using TaskManager.Services.Extensions.DatabaseSeeders;

namespace TaskManager.IntegrationTests.Scenarios.API
{
    [Collection("BaseCollection")]
    public class ProjectEditingTests
    {
        private readonly TestContext _testContext;
        private readonly TokenService _tokenService;
        private readonly string _user1SoloProjectId;
        private readonly string _sharedProjectId;
        private readonly HttpContent _requestToChangeShared;
        private readonly HttpContent _requestToChangeUser1Solo;

        public ProjectEditingTests(TestContext testContext)
        {
            _testContext = testContext;
            _tokenService = new TokenService();

            var projectNeedsRegenerating = _testContext.Context.Projects.Where(p => p.Title.Equals("projectToEdit")).Count() == 0;
            if (projectNeedsRegenerating)                                         
            {
                var assignee1 = _testContext.Context.Assignees.First(a => a.UserName.Equals("user1Name"));
                _testContext.Context.CreateTestProjectToEditFor(assignee1);
            }

            _user1SoloProjectId = _testContext.Context.Projects.First(p => p.Title.Equals("projectToEdit")).Id.ToString();
            _sharedProjectId = _testContext.Context.Projects.First(p => p.Title.Equals("sharedProject")).Id.ToString();

            _requestToChangeUser1Solo = ProjectWithCardsContentMaker.MakeStringContentWith(_user1SoloProjectId);
            _requestToChangeShared = ProjectWithCardsContentMaker.MakeStringContentWith(_sharedProjectId);
        }

        [Theory, MemberData(nameof(ProjectEditingEndpoints))]
        public async Task UserIsAuthorized_Change(params string[] urlAndAction)
        {
            AuthorizeClientAsUser("user1Name");

            var response = await RequestSender.Send(_testContext.Client, _requestToChangeUser1Solo, new string[] { urlAndAction[0] + _user1SoloProjectId, urlAndAction[1] });

            if (urlAndAction[1].Equals("DELETE"))
            {
                Assert.Equal(HttpStatusCode.NoContent.ToString(), response.StatusCode.ToString());
                Assert.True(_testContext.Context.Projects.Where(p => p.Id.ToString().Equals(_user1SoloProjectId)).Count() == 0);
            }
            else
            {
                var changedProject = _testContext.Context.Projects.First(p => p.Id.ToString().Equals(_user1SoloProjectId));
                Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
                Assert.Equal("changed", changedProject.Title);
            }
        }

        [Theory, MemberData(nameof(ProjectEditingEndpoints))]
        public async Task Project_IsNotOfUsers_Forbidden(params string[] urlAndAction)
        {
            AuthorizeClientAsUser("user2Name");

            var response = await RequestSender.Send(_testContext.Client, _requestToChangeUser1Solo, new string[] { urlAndAction[0] + _user1SoloProjectId, urlAndAction[1] });
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Forbidden.ToString(), response.StatusCode.ToString());
            Assert.Contains("only their own projects", responseString);
        }

        [Theory, MemberData(nameof(ProjectEditingEndpoints))]
        public async Task Project_IsShared_BadRequest(params string[] urlAndAction) 
        {
            AuthorizeClientAsUser("user1Name");

            var response = await RequestSender.Send(_testContext.Client, _requestToChangeShared, new string[] { urlAndAction[0] + _sharedProjectId, urlAndAction[1] });
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest.ToString(), response.StatusCode.ToString());
            Assert.Contains("The project is shared with other users. Only solo projects can be", responseString);
        }

        private void AuthorizeClientAsUser(string userName)
        {
            var user = _testContext.Context.Assignees.First(a => a.UserName.Equals(userName));
            var userToken = _tokenService.GenerateToken(user.Id, userName, "TodoUser");
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        }

        public static IEnumerable<object[]> ProjectEditingEndpoints
        {
            get
            {
                return new[]
                {
                    new string[] { "/api/users/me/projects/", "PUT" },
                    new string[] { "/api/users/me/projects/", "DELETE" },
                };
            }
        }
    }
}