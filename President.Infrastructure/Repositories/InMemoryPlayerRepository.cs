namespace President.Infrastructure.Repositories
{
    using President.Domain.Players;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private readonly List<Player> _players;

        public InMemoryPlayerRepository(IEnumerable<Player> players = null)
        {
            _players = players?.ToList() ?? new List<Player>();
        }

        public Task<Player> GetByIdAsync(PlayerId playerId)
        {
            return Task.FromResult(_players.FirstOrDefault(x => x.PlayerId.Equals(playerId)));
        }
    }
}
