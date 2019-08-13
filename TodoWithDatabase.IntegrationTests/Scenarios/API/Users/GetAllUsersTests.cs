using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests;
using TodoWithDatabase.IntegrationTests.Scenarios.API.Shared;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services;
using Xunit;

[Collection("UserCollection")]
public class GetAllUsersTests
{
    private readonly TestContext _testContext;

    public GetAllUsersTests(TestContext testContext)
    {
        _testContext = testContext;
        var tokenService = new TokenService();
        var correctToken = tokenService.GenerateToken("testAdmin", "TodoAdmin", false); // TODO this must be researched and changed to "true"
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

        Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
        Assert.Equal(expectedJson, response.Content.ReadAsStringAsync().Result);
    }
}