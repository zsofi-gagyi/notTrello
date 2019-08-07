using Microsoft.AspNetCore.Mvc;
using TodoWithDatabase.Services;
using Microsoft.AspNetCore.Authorization;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Controllers
{
    public class TodoController : Controller
    {
        private readonly IAssigneeService _assigneeService;
        private readonly ICardService _todoService;

        public TodoController(IAssigneeService assigneeService, ICardService todoService)
        {
            _todoService = todoService;
            _assigneeService = assigneeService;
        }
/*
        [HttpGet("users/addTodo")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        public IActionResult ShowAddTodo()
        {
            return View("Views/Web/AddTodo.cshtml");
        }

        [HttpPost("users/addTodo")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        [Authorize(Roles = "TodoUser")]
        public IActionResult DoAddTodo([FromForm]string title)
        {
            string name = User.Identity.Name;
            var assignee = _assigneeService.FindByName(name);
            _todoService.Save(title, assignee);
            return Redirect("/users");
        }

        [HttpGet("users/todo/{Id}/switchUrgent")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        public IActionResult switchUrgent([FromRoute(Name = "Id")]long id)
        {
            var todo = _todoService.getById(id);
            todo.Done = !todo.Urgent;
            _todoService.Save(todo);
            var assigneeId = todo.Assignee.Id;
            return Redirect("/users");
        }

        [HttpGet("users/todo/{Id}/switchDone")]
        [Authorize(Roles = "TodoUser" + "," + "TodoAdmin")]
        public IActionResult switchDone([FromRoute(Name="Id")]long id)
        {
            var todo = _todoService.getById(id);
            todo.Done = !todo.Done;
            _todoService.Save(todo);
            var assigneeId = todo.Assignee.Id;
            return Redirect("/users");
        }
        */
    }
}