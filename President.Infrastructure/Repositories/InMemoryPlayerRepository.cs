namespace President.Infrastructure.Repositories
{
    using President.Domain.Players;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private readonly List<Player> _players;

        public InMemoryPlayerRepository(Player player = null)
        {
            _players = new List<Player>();
            if (player != null)
                _players.Add(player);
        }

        public Task<Player> GetByIdAsync(PlayerId playerId)
        {
            return Task.FromResult(_players.FirstOrDefault(x => x.PlayerId.Equals(playerId)));
        }
    }
}
