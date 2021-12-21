namespace President.Domain.Players
{
    using System.Threading.Tasks;


    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(PlayerId playerId);
    }
}
