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
    }
}
