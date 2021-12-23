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
            var game = Game.FromState(new("g1", false, new Player[] { player }, new PlayerId[6]));
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
            var game = Game.FromState(new("g1", false, new Player[] { }, new PlayerId[6]));
            var gameRepository = new InMemoryGameRepository(game);
            var playerRepository = new InMemoryPlayerRepository(new List<Player> { player });
            var command = new RequestStartingGameCommand("p1", "g1");
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.DoesNotContain(game.AcceptedStartRequests, x => x.ToString().Equals("p1"));
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameNotExisting()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", false, new Player[] { }, new PlayerId[6]));
            var gameRepository = new InMemoryGameRepository();
            var playerRepository = new InMemoryPlayerRepository(new List<Player> { player });
            var command = new RequestStartingGameCommand("p1", "g1");
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.DoesNotContain(game.AcceptedStartRequests, x => x.ToString().Equals("p1"));
        }

        [Fact]
        public async Task PlayerUnknownCannotRequestStartingGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", false, new Player[] { player }, new PlayerId[6]));
            var gameRepository = new InMemoryGameRepository(game);
            var playerRepository = new InMemoryPlayerRepository(new List<Player> {  });
            var command = new RequestStartingGameCommand("p1", "g1");
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.DoesNotContain(game.AcceptedStartRequests, x => x.ToString().Equals("p1"));
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameWhenGameIsStarted()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", true, new Player[] { player }, new PlayerId[6]));
            var gameRepository = new InMemoryGameRepository(game);
            var playerRepository = new InMemoryPlayerRepository(new List<Player> { player });
            var command = new RequestStartingGameCommand("p1", "g1");
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.DoesNotContain(game.AcceptedStartRequests, x => x.ToString().Equals("p1"));
        }
    }
}
