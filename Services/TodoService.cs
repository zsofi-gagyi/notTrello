using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models;
using TodoWithDatabase.Repository;


namespace TodoWithDatabase.Services
{
    public class TodoService : ITodoService
    {
        readonly MyContext _myContext;

        public TodoService(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void Save(Todo todo)
        {
            if (_myContext.Todos.Contains(todo))
            {
                _myContext.Todos.Update(todo);
                
            } else {
                _myContext.Todos.Add(todo);
            }
            _myContext.SaveChanges();
        }

        public void Save(string task, Assignee assignee)
        {
            Todo todo = new Todo(task, assignee);
            Save(todo);
        }

        public List<Todo> GetAll()
        {
            return _myContext.Todos.Include("Assignee").ToList();
        }

        public List<Todo> GetAllBy(Assignee assignee)
        {
            return _myContext.Todos.Where(t => t.Assignee.Equals(assignee)).ToList();
        }

        public List<Todo> GetAllActive()
        {
            return _myContext.Todos.Where(t => !t.Done).ToList();
        }

        public Todo getById(long id)
        {
            return _myContext.Todos.Where(t => t.Id == id).Include(t => t.Assignee).FirstOrDefault();
        }
    }
}
