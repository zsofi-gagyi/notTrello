using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TaskManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using System;
using System.Linq;

namespace TaskManager.Controllers
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
        public async Task<IActionResult> DoSignUp([FromForm] string name, [FromForm]string password)
        {
            Assignee assignee = await _userManager.FindByNameAsync(name);

            if (assignee == null)
            {
                await _assigneeService.CreateAndSignInAsync(name, password);
            }

            return Redirect("/");
            // TODO: give more explicit (but not dangerously informative) feedback to users in case signing up has failed.
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LogIn([FromForm] string name, [FromForm]string password, [FromRoute]string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/");
            var result = await _signInManager.PasswordSignInAsync(name, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            return Redirect("/login");
            // TODO: give more explicit (but not dangerously informative) feedback to users in case the login has failed.
        }

        [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
        [HttpGet("/google-login")]
        public async Task<IActionResult> LogInWithGoogle()
        {
            var email = User
                .Claims
                .Where(cl => cl.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
                .First()
                .Value;

            Assignee assignee = await _userManager.FindByEmailAsync(email);

            if (assignee == null)
            {
                await _assigneeService.CreateAndSignInWithEmailAsync(User.Identity.Name, email);
                // This is not the best solution (two users with the same name could cause problems). 
                // To solve this properly, the entire sign up process should be changed (requiring an 
                // email address and verifying it), and then this email could be used as a key value
                // instead of the name, the way it is currently. 
            } 
            else
            {
                await _signInManager.SignInAsync(assignee, false, "googleAuth");
            }

            return Redirect("/");
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}