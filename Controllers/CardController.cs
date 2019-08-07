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
        private readonly IAssigneeService _assigneeService;
        private readonly ICardService _cardService;
        private readonly UserManager<Assignee> _userManager;

        public CardController(IProjectService projectService, IAssigneeService assigneeService, ICardService cardService, UserManager<Assignee> userManager)
        {
            _projectService = projectService;
            _assigneeService = assigneeService;
            _userManager = userManager;
            _cardService = cardService;
        }

        [HttpGet("/users/projects/{Id}/cards/addCard")]
        [Authorize]
        public IActionResult AddCard([FromRoute(Name = "Id")]string projectId)
        {
            string name = User.Identity.Name;
            var isAllowed = _projectService.userCollaboratesOnProject(name, projectId); // this will become middleware!

            if (isAllowed)
            {
                var project = _projectService.GetWithCards(projectId);
                ViewData.Add("project", project);
                return View();
            }

            return Redirect("/users");
        }

        [HttpPost("/users/projects/{Id}/cards/addCard")]
        [Authorize]
        public IActionResult DoAddCard([FromRoute(Name = "Id")]string projectId, [FromForm] Card card, List<string> collaboratorIds)
        {
            string name = User.Identity.Name;
            var isAllowed = _projectService.userCollaboratesOnProject(name, projectId); // this will become middleware!

            if (isAllowed)
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

            return Redirect("/users");
        }
    }
}