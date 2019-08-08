using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models;
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
            responsibles.Add(_userManager.GetUserAsync(User).Result);
            foreach (string id in collaboratorIds)
            {
                var collaborator = _userManager.FindByIdAsync(id).Result;
                responsibles.Add(collaborator);
            }

            foreach (Assignee responsible in responsibles)
            {
                card.AssigneeCards.Add(new AssigneeCard(responsible, card));
            }

            _cardService.Update(card);

            return Redirect("/users/projects/" + projectId);
        }

        [HttpGet("/users/projects/{Project.Id}/cards/{Id}/switchDone")]
        [Authorize]
        public IActionResult SwitchDone([FromRoute(Name = "Project.Id")] string projectId, [FromRoute(Name = "Id")] string cardId)
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