using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TaskManager.IntegrationTests.Fixtures;
using TaskManager.IntegrationTests.Fixtures.Context;
using Xunit;

namespace TaskManager.IntegrationTests.Scenarios.NotAPI.Shared
{

    [Collection("AlwaysAuthenticatedCollection")]
    public class HtmlOkWhenRoleNotNeededTests
    {
        private readonly AlwaysAuthenticatedTestContext _testContext;
        private readonly string _projectId;

        public HtmlOkWhenRoleNotNeededTests(AlwaysAuthenticatedTestContext testContext)
        {
            _testContext = testContext;
            _projectId = _testContext.Context.Projects.FirstOrDefault().Id.ToString();
        }

        [Theory, MemberData(nameof(HtmlEndpoints))]
        public async Task Get_Returns_Html_OK(params string[] urlParts)
        {
            string url = AssembleUrl(urlParts);
            var response = await _testContext.Client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory, MemberData(nameof(HtmlEndpoints))]
        public async Task Get_Returns_CorrectLayout(params string[] urlParts)
        {
            string url = AssembleUrl(urlParts);
            var response = await _testContext.Client.GetAsync(url);

            var content = await HtmlParser.Parse(response);
            var logo = content.QuerySelector("[data-test=logo]");

            Assert.Equal("TaskManager", logo.TextContent);
        }

        public static IEnumerable<object[]> HtmlEndpoints
        {
            get { 
             
                return new[]
                {   
                    new string[] { "/login" },
                    new string[] { "/signUp/" },
                    new string[] { "/APIguide" },
                    new string[] { "/users" },
                    new string[] { "/users/addProject" },
                    new string[] { "/users/projects/", "projectId"},
                    new string[] { "/users/projects/", "projectId", "/cards/addCard" },
                    new string[] { "/users/projects/", "projectId"},
                    new string[] { "/users/changeRole" },
                };
            }
        }

        private string AssembleUrl(string[] urlAndId)
        {
            if (urlAndId.Count() == 2)
            {
                return urlAndId[0] + _projectId;
            }
            else if (urlAndId.Count() == 3)
            {
                return urlAndId[0] + _projectId + urlAndId[2];
            }
            return urlAndId[0];
        }
    }
}