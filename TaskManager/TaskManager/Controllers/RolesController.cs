using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models.DAOs;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers
{
    [Route("/users")]
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

        [HttpPost("becomeAdmin")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TodoUser")]
        public async Task<IActionResult> BecomeAdmin([FromForm]string motivation)
        {
            var assignee = _assigneeService.GetWithAssigneeCards(User.Identity.Name);
            var cardsNr = assignee.AssigneeCards.Count();

            if (cardsNr > 0 && motivation.Length > 20)
            {
                await _userManager.AddToRoleAsync(assignee, "TodoAdmin");  
                await _signInManager.RefreshSignInAsync(assignee);
                ViewData.Add("role", "Admin");
                ViewData.Add("result", "You have been granted the title \"Admin\".");
            } else
            {
                ViewData.Add("result", "Your application for the title \"Admin\" has been rejected.");
                ViewData.Add("advice", "We recommend writing a detailed explanation of your motivation (min. 20 characters) " +
                    "and an active engagement with our community (min. 1 project with min. 1 card).");
            }

            return View("Views/Roles/result.cshtml");
        }

        [HttpGet("stopBeingAdmin")]
        [Authorize(Roles = "TodoAdmin")]
        public async Task<IActionResult> StopBeingAdmin()
        {
            var assignee = await _userManager.GetUserAsync(User);

            await _userManager.RemoveFromRoleAsync(assignee, "TodoAdmin");
            await _signInManager.RefreshSignInAsync(assignee);

            ViewData.Add("role", "User");
            ViewData.Add("result", "You have succesfully renounced of the title \"admin\".");
            return View("Views/Roles/result.cshtml");
        }
    }
}