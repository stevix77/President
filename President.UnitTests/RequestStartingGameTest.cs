namespace President.UnitTests
{
    using President.Application.Usecases.RequestStartingGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class RequestStartingGameTest
    {
        private readonly RequestStartingGameCommand _command;

        public RequestStartingGameTest()
        {
            _command = new RequestStartingGameCommand("p1", "g1");
        }
        [Fact]
        public async Task PlayerCanRequestToStartAGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", false, new Player[] { player }, new PlayerId[6]));
            var gameExpected = Game.FromState(new("g1", false, new Player[] { player }, new PlayerId[] { player.PlayerId }));
            await HandleWillAcceptRequest(new InMemoryGameRepository(game),
                                          new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerCannotAskStartingGameWhenNotInGame()
        {
            var game = Game.FromState(new("g1", false, Array.Empty<Player>(), new PlayerId[6]));
            var gameExpected = Game.FromState(new("g1", false, Array.Empty<Player>(), new PlayerId[6]));
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                          new InMemoryPlayerRepository(new List<Player> { new Player(new("p1")) }));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameNotExisting()
        {
            var game = Game.FromState(new("g1", false, Array.Empty<Player>(), new PlayerId[6]));
            var gameExpected = Game.FromState(new("g1", false, Array.Empty<Player>(), new PlayerId[6]));
            await HandleWillRejectRequest(new InMemoryGameRepository(),
                                          new InMemoryPlayerRepository(new List<Player> { new Player(new("p1")) }));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerUnknownCannotRequestStartingGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", false, new Player[] { player }, new PlayerId[6]));
            var gameExpected = Game.FromState(new("g1", false, new Player[] { player }, new PlayerId[6]));
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                          new InMemoryPlayerRepository(Array.Empty<Player>()));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameWhenGameIsStarted()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new("g1", true, new Player[] { player }, new PlayerId[6]));
            var gameExpected = Game.FromState(new("g1", true, new Player[] { player }, new PlayerId[6]));
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                        new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
        }

        private Task HandleWillAcceptRequest(InMemoryGameRepository gameRepository,
                                                    InMemoryPlayerRepository playerRepository)
        {
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            return handler.Handle(_command);
        }

        private Task HandleWillRejectRequest(InMemoryGameRepository gameRepository,
                                                    InMemoryPlayerRepository playerRepository)
        {
            var handler = new RequestStartingGameCommandHandler(gameRepository, playerRepository);
            return Record.ExceptionAsync(() => handler.Handle(_command));
        }
    }
}
