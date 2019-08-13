using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAssigneeService _assigneeService;
        private readonly SignInManager<Assignee> _signInManager;
        private readonly UserManager<Assignee> _userManager;

        public LoginController(IAssigneeService assigneeService, SignInManager<Assignee> signInManager, UserManager<Assignee> userManager)
        {
            _assigneeService = assigneeService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("/signUp")]
        public IActionResult DoSignUp([FromForm] string name, [FromForm]string password)
        {
            Assignee assignee = _userManager.FindByNameAsync(name).Result;

            if (assignee == null)
            {
                _assigneeService.CreateAndSignIn(name, password);
                return Redirect("/users");
            }

            return Redirect("/login.html");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LogIn([FromForm] string name, [FromForm]string password, [FromRoute]string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/users");
            var result = await _signInManager.PasswordSignInAsync(name,
                password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            return Redirect("/login.html");
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}