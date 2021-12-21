namespace President.UnitTests
{
    using President.Application.Usecases.JoinGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class JoinGameTest
    {
        [Fact]
        public async Task PlayerShouldJoinTheGame()
        {
            var game = new Game(new GameId("game1"));
            var repository = new InMemoryGameRepository(game);
            var command = new JoinGameCommand("player1", "game1");
            var handler = new JoinGameCommandHandler(repository, new InMemoryPlayerRepository(new Player(new PlayerId("player1"))));
            await handler.Handle(command);
            Assert.NotEmpty(game.Players);
        }

        [Fact]
        public async Task GameShouldNotAddPlayerUnknown()
        {
            var game = new Game(new GameId("game1"));
            var repository = new InMemoryGameRepository(game);
            var command = new JoinGameCommand("", "game1");
            var handler = new JoinGameCommandHandler(repository, new InMemoryPlayerRepository());
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.Empty(game.Players);
        }
    }

    internal class InMemoryPlayerRepository : IPlayerRepository
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
