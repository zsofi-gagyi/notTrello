using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Services;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class RolesController : Controller
    {
        private readonly IAssigneeService _assigneeService;
        private readonly UserManager<Assignee> _userManager;
        private readonly SignInManager<Assignee> _signInManager;

        public RolesController(IAssigneeService assigneeService, UserManager<Assignee> userManager, SignInManager<Assignee> signInManager)
        {
            _assigneeService = assigneeService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/users/changeRole")]
        [Authorize]
        public IActionResult ChangeRole()
        {
            return View();
        }

        [HttpPost("/users/becomeAdmin")]
        [Authorize(Roles = "TodoUser")]
        public IActionResult BecomeAdmin([FromForm]string motivation)
        {
            var assignee = _assigneeService.GetWithAssigneeCards(User.Identity.Name);
            var cardsNr = assignee.AssigneeCards.Count();

            if (cardsNr > 0 && motivation.Length > 20)
            {
                _userManager.AddToRoleAsync(assignee, "TodoAdmin").Wait(); //TODO research await and Wait() 
                _signInManager.RefreshSignInAsync(assignee).Wait();

                ViewData.Add("result", "You have been granted the title \"Admin\".");
            } else
            {
                ViewData.Add("result", "Your application for the title \"Admin\" has been rejected. <br><br>" +
                    "We recommend a detailed motivation (min. 20 characters) and a more active " +
                    "engagement with the community (min. 1 project with min. 1 card).");
            }

            return View("Views/Roles/result.cshtml");
        }

        [HttpGet("/users/stopBeingAdmin")]
        [Authorize(Roles = "TodoAdmin")]
        public IActionResult StopBeingAdmin()
        {
            var assignee = _userManager.GetUserAsync(User).Result;

            _userManager.RemoveFromRoleAsync(assignee, "TodoAdmin").Wait();
            _signInManager.RefreshSignInAsync(assignee).Wait();

            ViewData.Add("result", "You have succesfully renounced of the title \"admin\".");
            return View("Views/Roles/result.cshtml");
        }
    }
}