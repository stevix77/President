namespace President.Domain.Games
{
    using System.Threading.Tasks;


    public interface IGameRepository
    {
        Task SaveAsync(Game game);
        Task<Game> GetByIdAsync(GameId gameId);
    }
}
