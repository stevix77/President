namespace President.UnitTests
{
    using President.Application.Usecases.RequestStartingGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Domain.Players.Events;
    using President.Infrastructure.Repositories;
    using President.UnitTests.Builders;
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
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .WithRequesters(new[] { player.PlayerId })
                       .Build());
            await HandleWillAcceptRequest(new InMemoryGameRepository(game),
                                          new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Contains(player.DomainEvents, x => x.ToString() == new StartRequested(player.PlayerId, game.GameId).ToString());
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameWhenNotInGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new GameStateBuilder()
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                                                    .Build());
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                          new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameNotExisting()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new GameStateBuilder()
                                            .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                                                .Build());
            await HandleWillRejectRequest(new InMemoryGameRepository(),
                                          new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        [Fact]
        public async Task PlayerUnknownCannotRequestStartingGame()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .Build());
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                          new InMemoryPlayerRepository(Array.Empty<Player>()));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameWhenGameIsStarted()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .WithHasBegan(true)
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .WithHasBegan(true)
                       .Build());
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                        new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        [Fact]
        public async Task PlayerCannotRequestStartingGameWhenHasAlreadyRequested()
        {
            var player = new Player(new("p1"));
            var game = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .WithRequesters(new[] {player.PlayerId})
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithPlayers(new[] { player })
                       .WithRequesters(new[] { player.PlayerId })
                       .Build());
            await HandleWillRejectRequest(new InMemoryGameRepository(game),
                                        new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
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
