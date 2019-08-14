using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Models.DAOs.JoinTables;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class CardController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICardService _cardService;
        private readonly UserManager<Assignee> _userManager;

        public CardController(IProjectService projectService, ICardService cardService, UserManager<Assignee> userManager)
        {
            _projectService = projectService;
            _cardService = cardService;
            _userManager = userManager;
        }

        [HttpGet("/users/projects/{Id}/cards/addCard")]
        [Authorize]
        public IActionResult AddCard([FromRoute(Name = "Id")]string projectId)
        {
            var project = _projectService.GetWithCards(projectId);
            ViewData.Add("project", project);
            return View();
        }

        [HttpPost("/users/projects/{Id}/cards/addCard")]
        [Authorize]
        public IActionResult DoAddCard([FromRoute(Name = "Id")]string projectId, [FromForm] Card card, List<string> collaboratorIds)
        {
            _cardService.Save(card);

            var project = _projectService.Get(projectId);
            card.Project = project;

            var responsibles = new List<Assignee>();
            var user = _userManager.GetUserAsync(User).Result;
            responsibles.Add(user);
            foreach (string id in collaboratorIds)
            {
                var collaborator = _userManager.FindByIdAsync(id).Result;
                responsibles.Add(collaborator);
            }

            var newCards = new List<AssigneeCard>();
            foreach (Assignee responsible in responsibles)
            {
                newCards.Add(new AssigneeCard(responsible, card));
            }

            card.AssigneeCards.AddRange(newCards);
            _cardService.Update(card);

           // user.AssigneeCards.AddRange(newCards);
            _userManager.UpdateAsync(user).Wait();

            return Redirect("/users/projects/" + projectId);
        }

        [HttpGet("/users/projects/{Project.Id}/cards/{Card.Id}/toggleDone")]
        [Authorize]
        public IActionResult ToggleDone([FromRoute(Name = "Project.Id")] string projectId, [FromRoute(Name = "Card.Id")] string cardId)
        {
            var project = _projectService.GetWithCards(projectId);
            var card = project.Cards.Where(c => c.Id.ToString().Equals(cardId)).FirstOrDefault();

            if (card != null)
            {
                card.Done = !card.Done;
                _cardService.Update(card);
            }
            return Redirect("/users/projects/" + projectId);
        }
    }
}