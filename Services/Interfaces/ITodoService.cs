using System.Collections.Generic;
using TodoWithDatabase.Models;

namespace TodoWithDatabase.Services
{
    public interface ITodoService
    {
        void Save(Todo todo);
        void Save(string task, Assignee assignee);
        List<Todo> GetAll();
        List<Todo> GetAllBy(Assignee assignee);
        List<Todo> GetAllActive();

        Todo getById(long id);
    }
}
