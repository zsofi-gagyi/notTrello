using TaskManager.Models.DAOs;

namespace TaskManager.Services.Interfaces
{
    public interface ICardService
    {
        void Save(Card card);

        void Update(Card card);
    }
}
