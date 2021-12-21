using President.Domain.Games;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.Infrastructure.Repositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> _games;
        public InMemoryGameRepository(Game game = null)
        {
            _games = new List<Game>();
            if (game != null)
                _games.Add(game);
        }
        public Task SaveAsync(Game party)
        {
            _games.Add(party);
            return Task.CompletedTask;
        }

        public Game GetGame(string gameId)
        {
            return _games.Any() ? _games[0] : null;
        }

        public int CountGames() => _games.Count;

        public Task<Game> GetByIdAsync(GameId partyId)
        {
            return Task.FromResult(_games.FirstOrDefault(x => x.GameId.Equals(partyId)));
        }
    }
}
