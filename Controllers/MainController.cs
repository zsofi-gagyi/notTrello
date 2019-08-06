using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Models;
using TodoWithDatabase.Services;

namespace TodoWithDatabase.Controllers
{
    public class MainController : Controller
    {
        private readonly ITodoService _todoService;

        public MainController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("/")]
        public IActionResult NotLoggedInTodoList()
        {
            ViewData["TodoList"] = _todoService.GetAll();
            return View("Views/Web/TodoList.cshtml");
        }
                                               
        [HttpGet("/users")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        public IActionResult TodoList()
        {
            ViewData["TodoList"] = _todoService.GetAll();
            return View("Views/Web/TodoList.cshtml");
        }
    }
}
