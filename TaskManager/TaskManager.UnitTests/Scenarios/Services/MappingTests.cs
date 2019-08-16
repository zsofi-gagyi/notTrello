using AutoMapper;
using Newtonsoft.Json;
using TodoWithDatabase.Models.DTO;
using TodoWithDatabase.Services.Extensions;
using TodoWithDatabase.UnitTests.Fixtures.TestObjectMakers;
using Xunit;

namespace TodoWithDatabase.UnitTests.Services
{
    public class MappingTests  
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _mapper = AutoMapperSetup.CreateMapper();
        }

        [Fact]
        public void Assignee_To_AssigneeWithProjectsDTO_Translates()
        {
            var assignee = AssigneeMaker.MakeAssigneeWithProject();

            var expectedDTO = AssigneeWithProjectsDTOMaker.MakeFrom(assignee);
            var expectedString = JsonConvert.SerializeObject(expectedDTO);

            var assigneeDTO = _mapper.Map<AssigneeWithProjectsDTO>(assignee);
            var resultString = JsonConvert.SerializeObject(assigneeDTO);

            Assert.Equal(expectedString, resultString);
        }

        /*
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
        */
    }
}
