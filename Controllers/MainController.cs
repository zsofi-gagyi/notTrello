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
        private readonly ICardService _todoService;

        public MainController(ICardService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("/")]
        public IActionResult MainPage()
        {
            return View();
        }
                                               
        [HttpGet("/users")]
        [Authorize]
        public IActionResult TodoList()
        {
           // ViewData["TodoList"] = _todoService.GetAll();
            return View();
        }
    }
}
