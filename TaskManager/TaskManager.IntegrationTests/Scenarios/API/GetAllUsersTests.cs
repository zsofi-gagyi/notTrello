using Newtonsoft.Json;
using System.Collections.Generic;
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

    [Collection("ProjectsCollection")]
    public class GetAllUsersTests
    {
        private readonly TestContext _testContext;

        public GetAllUsersTests(TestContext testContext)
        {
            _testContext = testContext;
            var tokenService = new TokenService();
            var correctToken = tokenService.GenerateToken("testID", "testAdmin", "TodoAdmin"); 
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", correctToken);
        }

        [Theory]
        [InlineData("/api/users/all")]
        public async Task Get_ReturnsAllUsers(string url)
        {
            var testUserId = _testContext.Context.Assignees.First(a => a.UserName.Equals("user1Name")).Id.ToString();
            var testUser = new AssigneeDTO { UserName = "user1Name", Id = testUserId };
            var expectedList = new List<AssigneeDTO> { testUser };
            var expectedJson = JsonConvert.SerializeObject(expectedList);

            var response = await _testContext.Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());

            var actualusers = response.Content.ReadAsStringAsync().Result;
            Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(List<AssigneeDTO>)));
        }
    }
}