using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using TodoWithDatabase.Models;
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
        [Authorize(Roles = "TodoUser,TodoAdmin")]
        public IActionResult ChangeRole()
        {
            return View();
        }

        [HttpPost("/users/becomeAdmin")]
        [Authorize(Roles = "TodoUser")]
        public IActionResult BecomeAdmin([FromForm]string motivation)
        {
            string name = User.Identity.Name;
            var assignee = _assigneeService.FindByName(name);
            int todosNr = 0; //assignee.AssigneeCards.Count();

            if (todosNr >= 0 && motivation.Length > 7)
            {
                _userManager.AddToRoleAsync(assignee, "TodoAdmin").Wait(); //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2#react-to-back-end-changes
                _signInManager.RefreshSignInAsync(assignee).Wait();

                ViewData.Add("result", "You have been granted the title \"admin\".");
            } else
            {
                ViewData.Add("result", "Your application for the title \"admin\" has been rejected. " +
                    "We recommend a detailed motivation (min. 70 characters) and a more active " +
                    "engagement with the community (min. 3 cards)");
            }

            return View("Views/Roles/result.cshtml");
        }

        [HttpGet("/users/stopBeingAdmin")]
        [Authorize(Roles = "TodoAdmin")]
        public IActionResult StopBeingAdmin()
        {
            string name = User.Identity.Name;
            var assignee = _assigneeService.FindByName(name);

            _userManager.RemoveFromRoleAsync(assignee, "TodoAdmin").Wait();
            _signInManager.RefreshSignInAsync(assignee).Wait();

            ViewData.Add("result", "You have succesfully renounced of the title \"admin\".");
            return View("Views/Roles/result.cshtml");
        }
    }
}