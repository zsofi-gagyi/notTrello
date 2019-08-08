using TodoWithDatabase.Models.DAOs;

namespace TodoWithDatabase.Services.Interfaces
{
    public interface ICardService
    {
        void Save(Card card);

        void Update(Card card);
    }
}
