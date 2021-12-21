namespace President.Domain.Games
{
    using System.Threading.Tasks;


    public interface IGameRepository
    {
        Task SaveAsync(Game party);
        Task<Game> GetByIdAsync(GameId partyId);
    }
}
