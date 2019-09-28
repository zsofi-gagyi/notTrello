using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.DAOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TaskManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using System.Linq;
using TaskManager.Services.Extensions;

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
        public async Task<IActionResult> SignUp([FromForm] string name, [FromForm]string password)
        {
            if (password.IsIncorrectPassword())
            {
                TempData["errorMessage"] = "Passwords must be a single word, of at least 8 characters, containing one number, one uppercase and one lowercase letter";
                return Redirect("/signUp");
            }

            if (name.IsIncorrectUserName())
            {
                TempData["errorMessage"] = "Names should be a single word";
                return Redirect("/signUp");
            }

            Assignee assignee = await _userManager.FindByNameAsync(name);
            if (assignee == null)
            {
                await _assigneeService.CreateAndSignInAsync(name, password);
                return Redirect("/");
            } 

            TempData["errorMessage"] = "Username already taken"; // If an email adresses were required of ever user, we could hide this 
                                                                 // information and just send a verifying email instead.
            return Redirect("/signUp");

        }

        [HttpPost("/login")]
        public async Task<IActionResult> LogIn([FromForm] string name, [FromForm]string password, [FromRoute]string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/");
            var result = await _signInManager.PasswordSignInAsync(name, password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            TempData["errorMessage"] = "Username or password is incorrect.";
            return Redirect("/login");
        }

        [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
        [HttpGet("/google-login")]
        public async Task<IActionResult> LogInWithGoogle()
        {
            var email = User
                .Claims
                .First(cl => cl.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
                .Value;

            Assignee assignee = await _userManager.FindByEmailAsync(email);
            if (assignee == null)
            {
                await _assigneeService.CreateAndSignInWithEmailAsync(User.Identity.Name, email);
                // This is not the best solution (two users with the same legal name could cause problems). 
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