using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Scenarios.API.Shared;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Users
{
    [Collection("BaseCollection")]
    public class AddUsersTest 
    {
        private readonly TestContext _testContext;

        public AddUsersTest(TestContext testContext)
        {
            _testContext = testContext;
            var tokenService = new TokenService();
            var correctToken = tokenService.GenerateToken("testAdmin", "TodoAdmin", false); // TODO this must be researched and changed to "true"
            _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", correctToken);
        }

        [Theory]
        [InlineData("/api/users")]
        public async Task Post_CorrectDTO_CreatesNewUser(string url)
        {
            var assigneeDTO = new AssigneeToCreateDTO { Name = "testName1", Password = "testPassword" };
            var assigneeJson = JsonConvert.SerializeObject(assigneeDTO);
            HttpContent request = new StringContent(assigneeJson, Encoding.UTF8, "application/json");

            var expectedResponse = new { message = "User with the name testName1 has been succesfully created!" };
            var expectedResponseString = JsonConvert.SerializeObject(expectedResponse);

            var response = await _testContext.Client.PostAsync(url, request);

            Assert.Equal(HttpStatusCode.Created.ToString(), response.StatusCode.ToString());
            Assert.Equal(expectedResponseString, response.Content.ReadAsStringAsync().Result);
        }

        [Theory]
        [InlineData("/api/users")]
        public async Task Post_IncorrectDTOFormat_BadRequest(string url)
        {
            var assigneeDTO = new { Name = "testName2" };
            var assigneeJson = JsonConvert.SerializeObject(assigneeDTO);
            HttpContent request = new StringContent(assigneeJson, Encoding.UTF8, "application/json");

            var response = await _testContext.Client.PostAsync(url, request);

            Assert.Equal(HttpStatusCode.BadRequest.ToString(), response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/api/users")]
        public async Task Post_AlreadyExistingUser_BadRequest(string url)
        {
            var assigneeDTO = new { Name = "testName2" };
            var assigneeJson = JsonConvert.SerializeObject(assigneeDTO);
            HttpContent request = new StringContent(assigneeJson, Encoding.UTF8, "application/json");

            var response = await _testContext.Client.PostAsync(url, request);

            Assert.Equal(HttpStatusCode.BadRequest.ToString(), response.StatusCode.ToString());
        }
    }
}
