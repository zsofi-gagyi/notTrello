using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoWithDatabase.Models;
using TodoWithDatabase.Models.DAOs;
using TodoWithDatabase.Repository;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Services
{
    public class CardService : ICardService
    {
        readonly MyContext _myContext;

        public CardService(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void Save(Card card)
        {
            _myContext.Cards.Add(card);
            _myContext.SaveChanges();
        }

        public void Update(Card card)
        {
            _myContext.Cards.Update(card);
            _myContext.SaveChanges();
        }

        public void Save(string task, Assignee assignee)
        {
            Card todo = new Card(/*task, assignee*/);
            Save(todo);
        }

        public List<Card> GetAll()
        {
            return _myContext.Cards.Include("Assignee").ToList();
        }

        public List<Card> GetAllBy(Assignee assignee)
        {
            return _myContext.Cards/*.Where(t => t.Assignee.Equals(assignee))*/.ToList();
        }

        public List<Card> GetAllActive()
        {
            return _myContext.Cards.Where(t => !t.Done).ToList();
        }

        public Card getById(long id)
        {
            return _myContext.Cards/*.Where(t => t.Id == id).Include(t => t.Assignee)*/.FirstOrDefault();
        }
    }
}
