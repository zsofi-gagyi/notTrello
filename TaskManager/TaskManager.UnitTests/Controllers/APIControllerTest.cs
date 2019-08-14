using Moq;
using TodoWithDatabase.Models;
using TodoWithDatabase.Services;
using Xunit;
using Newtonsoft.Json.Linq;
using TodoWithDatabase.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TodoWithDatabase.UnitTests
{

    public class APIControllerTest
    { /*
            Mock<IAssigneeService> mockAssigneeService;
            APIController controller;
            JObject testData;
            Assignee testAssignee;

            public APIControllerTest()
            {
                this.mockAssigneeService = new Mock<IAssigneeService>();
                this.testData = JObject.FromObject(new { UserName = "testName", PasswordHash = "testPassword" });
                this.testAssignee = new Assignee { UserName = "testName", PasswordHash = "testPassword" };
                this.controller = new APIController(this.mockAssigneeService.Object);
            }

            [Fact]
            public void Creates_NewAssignee()
            {
                IActionResult expected = new JsonResult(new { message = "assignee named testName succesfully created!" });
                IActionResult result = this.controller.AddAssignee(this.testData);

                result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
            }

            [Fact]
            public void DoesNotCreate_NewAssignee()
            {
                this.mockAssigneeService.Setup(p => p.FindByName("testName")).Returns(this.testAssignee);

                IActionResult result = this.controller.AddAssignee(this.testData);
                IActionResult expected = new JsonResult(new { message = "assignee already exists!" });
                result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
            }

            [Fact]
            public void Saves_Once()
            {
                IActionResult result = this.controller.AddAssignee(this.testData);
                this.mockAssigneeService.Verify(m => m.Save(It.IsAny<Assignee>()), Times.Once());
            }
        
        */
    }
}
    
