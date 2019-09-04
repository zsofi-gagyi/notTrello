﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.DAOs;
using TaskManager.Models.DAOs.JoinTables;
using TaskManager.Models.ViewModels;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers
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
            var viewModel = new ProjectViewModel();
            viewModel.project = _projectService.GetWithCards(projectId);
            return View(viewModel);
        }

        [HttpPost("/users/projects/{Id}/cards/addCard")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddCard([FromRoute(Name = "Id")]string projectId, [FromForm] Card card, List<string> collaboratorIds)
        {
            var project = _projectService.Get(projectId);
            if (project == null)
            {
                return BadRequest();
            }

            _cardService.Save(card);

            card.Project = project;

            var responsibles = new List<Assignee>();
            var user = await _userManager.GetUserAsync(User);
            responsibles.Add(user);
            foreach (string id in collaboratorIds)
            {
                var collaborator = await _userManager.FindByIdAsync(id);
                responsibles.Add(collaborator);
            }

            var newCards = new List<AssigneeCard>();
            foreach (Assignee responsible in responsibles)
            {
                newCards.Add(new AssigneeCard(responsible, card));
            }

            card.AssigneeCards.AddRange(newCards);
            _cardService.Update(card);

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