using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TodoWithDatabase.Models;
using TodoWithDatabase.Services;

namespace TodoWithDatabase.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAssigneeService _assigneeService;
        private readonly UserManager<Assignee> _userManager;
        private readonly SignInManager<Assignee> _signInManager;

        public AdminController(IAssigneeService assigneeService, UserManager<Assignee> userManager, SignInManager<Assignee> signInManager)
        {
            _assigneeService = assigneeService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("/users/becomeAdmin")]
        [Authorize(Roles = "TodoUser,TodoAdmin")]
        public IActionResult Result([FromForm]string motivation)
        {
            string name = User.Identity.Name;
            var assignee = _assigneeService.FindByName(name);
            int todosNr = assignee.Todos.Count();

            if (todosNr > 3 && motivation.Length > 70)
            {
                _userManager.AddToRoleAsync(assignee, "TodoAdmin").Wait();
                _userManager.RemoveFromRoleAsync(assignee, "TodoUser").Wait();
                _signInManager.SignOutAsync().Wait();
                _userManager.UpdateSecurityStampAsync(assignee).Wait();
                _signInManager.SignInAsync(assignee, false).Wait();

            }

            return View("Views/Web/adminResult.cshtml");
        }
    }
}
