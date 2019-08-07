using System.Collections.Generic;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface ICardService
    {
        void Save(Card card);

        void Update(Card card);
        void Save(string task, Assignee assignee);
        List<Card> GetAll();
        List<Card> GetAllBy(Assignee assignee);
        List<Card> GetAllActive();

        Card getById(long id);
    }
}
