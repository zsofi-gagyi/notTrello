using Xunit;
using TodoWithDatabase.Services.Extensions;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DTO;
using AutoMapper;
using System.Collections.Generic;
using Moq;

namespace TodoWithDatabase.UnitTests.Services.Extensions
{
    public class AssigneeTranslatorTest
    {

        Mock<IMapper> _mockMapper;
        Assignee _testAssignee;

        public AssigneeTranslatorTest()
        {
            _mockMapper = new Mock<IMapper>();

            Assignee testAssignee = new Assignee { UserName = "testName", PasswordHash = "testPassword" };
            Todo testTodo1 = new Todo("testTitle1", testAssignee);
            Todo testTodo2 = new Todo("testTitle2", testAssignee);
            testAssignee.Todos.AddRange(new List<Todo>() { testTodo1, testTodo2 });
            _testAssignee = testAssignee;
        }


        [Fact]
        public void Translator_GivesBack_CorrectFormat()
        {
            _mockMapper.Setup(p => p.Map<TodoDTO>(It.IsAny<Todo>())).Returns(new TodoDTO());
            var translated = _testAssignee.ToDtoUsingMapper(_mockMapper.Object);
            Assert.True(translated is AssigneeDTO);
        }

        [Fact]
        public void Translator_GivesBack_CorrectContent()
        {
            this._mockMapper.SetupSequence(p => p.Map<TodoDTO>(It.IsAny<Todo>()))
                .Returns(new TodoDTO() { Title = "testTitle1" })
                .Returns(new TodoDTO() { Title = "testTitle2" });

            AssigneeDTO translated = _testAssignee.ToDtoUsingMapper(_mockMapper.Object);
            Assert.Equal(_testAssignee.UserName, translated.Name);
            Assert.Equal(2, translated.Todos.Count);
            Assert.Equal("testTitle1", translated.Todos[0].Title);
            Assert.Equal("testTitle2", translated.Todos[1].Title);
        }
    }
}
