using TaskManager.Models.DAOs;
using TaskManager.Repository;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services
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
    }
}
