using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;
using TaskManager.IntegrationTests.Fixtures.TestObjectMakers;
using TodoWithDatabase.Controllers;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DTOs;
using TodoWithDatabase.Services.Extensions;
using TodoWithDatabase.Services.Interfaces;
using TodoWithDatabase.UnitTests.Fixtures.TestObjectMakers;

namespace TaskManager.UnitTests.Scenarios.Controllers
{
    public class CardControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly Mock<ICardService> _mockCardService;
        private readonly CardController _controller;

        public CardControllerTests()
        {
            _mapper = AutoMapperSetup.CreateMapper();
            _mockProjectService = new Mock<IProjectService>();
            _mockProjectService
                .Setup(p => p.Get("correctProjectId"))
                .Returns(ProjectMaker.Make());

            _mockCardService = new Mock<ICardService>();

            var mockUserManager = MakeMockUserManager(); 
            _controller = new CardController(_mockProjectService.Object, _mockCardService.Object, mockUserManager.Object);
        }

        [Fact]
        public void CorrectInput_OneCollaborator_Saves_Once()
        {
            _controller.DoAddCard("correctProjectId", CardMaker.MakeOriginal(), new List<string> { "collaboratorId" });

            _mockCardService.Verify(m => m.Save(It.IsAny<Card>()), Times.Once());
        }

        [Fact]
        public void CorrectInput_ZeroCollaborator_Saves_Once()
        {
            _controller.DoAddCard("correctProjectId", CardMaker.MakeOriginal(), new List<string> { });

            _mockCardService.Verify(m => m.Save(It.IsAny<Card>()), Times.Once());
        }

        [Fact]
        public void CorrectInput_Updates_CompletedCard()
        {
            _controller.DoAddCard("correctProjectId", CardMaker.MakeOriginal(), new List<string> { "collaboratorId" });

            _mockCardService.Verify(m => m.Update(It.IsAny<Card>()), Times.Once());
            _mockCardService.Verify(m => m.Update(It.Is<Card>(c => CardsAreEqual(c, CardMaker.MakeCompleted()))), Times.Once());
        }

        [Fact]
        public void IncorrectProjectId_DoesNotSave()
        {
            _controller.DoAddCard("incorrectProjectId", CardMaker.MakeOriginal(), new List<string> { "collaboratorId" });
            _mockCardService.Verify(m => m.Save(It.IsAny<Card>()), Times.Never());
        }

        private Mock<UserManager<Assignee>> MakeMockUserManager()
        {
            var expectedUser = AssigneeMaker.MakeUser();  
            var expectedCollaborator = AssigneeMaker.MakeCollaborator();  

            var mockUserStore = new Mock<IUserStore<Assignee>>();
            var mockUserManager = new Mock<UserManager<Assignee>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Object.UserValidators.Add(new UserValidator<Assignee>());
            mockUserManager.Object.PasswordValidators.Add(new PasswordValidator<Assignee>());

            mockUserManager.Setup(x => x.GetUserAsync(null)).ReturnsAsync(expectedUser);
            mockUserManager.Setup(x => x.FindByIdAsync("collaboratorId")).ReturnsAsync(expectedCollaborator);

            return mockUserManager;
        }

        private bool CardsAreEqual(Card card1, Card card2) 
        {
            var cardDTO1 = _mapper.Map<CardWithProjectDTO>(card1);
            var cardDTO2 = _mapper.Map<CardWithProjectDTO>(card2);

            var cardString1 = JsonConvert.SerializeObject(cardDTO1);
            var cardString2 = JsonConvert.SerializeObject(cardDTO2);

            return cardString1.Equals(cardString2);
        }
    }
}
