using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.App.TodoWithDatabase.IntegrationTests.Fixtures;
using TodoWithDatabase.Models;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Users
{

    [Collection("ProjectsCollection")]
    public class GetAllUsersTests
    {
        private readonly TestContext _testContext;

        public GetAllUsersTests(TestContext testContext)
        {
            _testContext = testContext;
            var tokenService = new TokenService();
            var correctToken = tokenService.GenerateToken("testID", "testAdmin", "TodoAdmin"); // TODO this must be researched and changed to "true"
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", correctToken);
        }

        [Theory]
        [InlineData("/api/users/all")]
        public async Task Get_ReturnsAllUsers(string url)
        {
            var testUserId = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault().Id.ToString();
            var testUser = new AssigneeDTO { UserName = "user1Name", Id = testUserId };
            var expectedList = new List<AssigneeDTO> { testUser };
            var expectedJson = JsonConvert.SerializeObject(expectedList);

            var response = await _testContext.Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());

            var actualusers = response.Content.ReadAsStringAsync().Result;
            //Assert.Equal(expectedJson, response.Content.ReadAsStringAsync().Result);
            Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(List<AssigneeDTO>)));
        }
    }
}