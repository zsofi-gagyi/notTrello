using System.Linq;
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

            var projectTitle = content.QuerySelector("[data-test=projectTitle]").TextContent;
            var projectDescription = content.QuerySelector("[data-test=projectDescription]").TextContent;

            var cardsToDoNumber = content.QuerySelectorAll("[data-test=toDoCard]").Length;
            var cardsDoneNumber = content.QuerySelectorAll("[data-test=doneCard]").Length;
            var cardToDoTitle = content.QuerySelector("[data-test=toDoCard] > [data-test=title]").TextContent;
            var cardDoneDescription = content.QuerySelector("[data-test=doneCard] > [data-test=description]").TextContent;

            var cardDoneResponsibles = content.QuerySelectorAll("[data-test=doneCard] >> [data-test=responsibleName]");

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