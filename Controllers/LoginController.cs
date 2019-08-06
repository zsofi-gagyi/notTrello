using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services;
using TodoWithDatabase.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TodoWithDatabase.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAssigneeService _assigneeService;
        private readonly SignInManager<Assignee> _signInManager;

        public LoginController(IAssigneeService assigneeService,
            SignInManager<Assignee> signInManager)
        {
            _assigneeService = assigneeService;
            _signInManager = signInManager;
        }

        [HttpPost("/signUp")]
        public IActionResult DoSignUp([FromForm] string name, [FromForm]string password, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/users");
            Assignee assignee = _assigneeService.FindByName(name);

            if (assignee == null)
            {
                _assigneeService.SaveNew(name, password);

                return LocalRedirect(returnUrl);
            }

            return Redirect("/login.html");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> DoLogIn([FromForm] string name, [FromForm]string password, string returnUrl = null)
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
        public async Task<IActionResult> DoLogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}