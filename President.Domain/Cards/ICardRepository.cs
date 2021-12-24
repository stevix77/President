using System.Threading.Tasks;

namespace President.Domain.Cards
{
    public interface ICardRepository
    {
        Task<Card[]> GetCards();
    }
}
