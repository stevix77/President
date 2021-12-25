namespace President.UnitTests
{
    using President.Application.Usecases.RequestStartingGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class RequestStartingGameTest
    {
        [Fact]
        public async Task PlayerCanRequestToStartAGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", false, new Player[] { player }));
            var gameRepository = new InMemoryGameRepository(game);
            var playerRepository = new InMemoryPlayerRepository(new List<Player> { player });
            var command = new RequestStartingGameCommand("p1", "g1");
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            await handler.Handle(command);
            Assert.Contains(game.AcceptedStartRequests, x => x.ToString().Equals("p1"));
        }

        [Fact]
        public async Task PlayerCannotAskStartingGameWhenNotInGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", false, new Player[] { }));
            var gameRepository = new InMemoryGameRepository(game);
            var playerRepository = new InMemoryPlayerRepository(new List<Player> { player });
            var command = new RequestStartingGameCommand("p1", "g1");
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            await handler.Handle(command);
            Assert.DoesNotContain(game.AcceptedStartRequests, x => x.ToString().Equals("p1"));
        }
    }
}
