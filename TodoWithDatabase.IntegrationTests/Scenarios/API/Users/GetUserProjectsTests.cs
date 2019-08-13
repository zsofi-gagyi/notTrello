using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoWithDatabase.App.TodoWithDatabase.IntegrationTests.Fixtures;
using TodoWithDatabase.IntegrationTests;
using TodoWithDatabase.Models.DTO;
using TodoWithDatabase.Services;
using Xunit;

[Collection("UserCollection")]
public class GetUserProjectsTests
{
    private readonly TestContext _testContext;
    private readonly string _user1Id;

    public GetUserProjectsTests(TestContext testContext)
    {
        _testContext = testContext;
        var tokenService = new TokenService();
        var correctToken = tokenService.GenerateToken("testAdmin", "TodoAdmin", false);
        _testContext.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", correctToken);
        _user1Id = _testContext.Context.Assignees.Where(a => a.UserName.Equals("user1Name")).FirstOrDefault().Id;
    }

    [Theory]
    [InlineData("/api/users/", "/userWithProjects")]
    public async Task Get_CorrectId_ReturnsUserWithProjects(params string[] urlParts)
    {
        var response = await _testContext.Client.GetAsync(urlParts[0] + _user1Id + urlParts[1]);
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
        Assert.True(FormatVerifier.StringIsValidAs(responseString, typeof(AssigneeWithProjectsDTO)));
    }
}