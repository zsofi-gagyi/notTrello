using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TodoWithDatabase.IntegrationTests.Helpers;
using Xunit;

namespace TodoWithDatabase.IntegrationTests.Scenarios.API.Users
{

    [Collection("AlwaysAuthenticatedCollection")]
    public class ProjectControllerTests
    {
        private readonly AlwaysAuthenticatedTestContext _testContext;
        private readonly string _projectId;

        public ProjectControllerTests(AlwaysAuthenticatedTestContext testContext)
        {
            _testContext = testContext;
            _projectId = _testContext.Context.Projects.FirstOrDefault().Id.ToString();
        }

        [Theory]
        [InlineData("/users/projects/")]
        public async Task Get_Returns_CorrectProject(string url)
        {
            var response = await _testContext.Client.GetAsync(url + _projectId);

            var content = await HtmlParser.Parse(response);

            var projectTitle = content.QuerySelector(".mainTitle").TextContent;
            var projectDescription = content.QuerySelector(".primaryContainer li.description").TextContent;

            var cardsToDoNumber = content.QuerySelectorAll(".toDoCard").Length;
            var cardsDoneNumber = content.QuerySelectorAll(".doneCard").Length;
            var cardToDoTitle = content.QuerySelector(".toDoCard li.title").TextContent;
            var cardDoneDescription = content.QuerySelector(".doneCard li.description").TextContent;

            var cardDoneResponsibles = content.QuerySelectorAll(".doneCard li.responsibles span.responsibleName");

            Assert.Equal("project title", projectTitle);
            Assert.Equal("project description", projectDescription);
            Assert.Equal(1, cardsToDoNumber);
            Assert.Equal(1, cardsDoneNumber);
            Assert.Equal("todo card title", cardToDoTitle);
            Assert.Equal("done card description", cardDoneDescription);

            Assert.Equal(2, cardDoneResponsibles.Length);
            Assert.Equal("user1", cardDoneResponsibles[0].TextContent);
            Assert.Equal("user2", cardDoneResponsibles[1].TextContent);
        }
    }
}