namespace President.UnitTests
{
    using President.Application.Usecases.PlayCards;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using President.UnitTests.Builders;
    using System;
    using System.Threading.Tasks;
    using Xunit;


    public class PlayCardsTest
    {
        [Fact]
        public async Task PlayFirstOneCardShouldBeValid()
        {
            var player = new Player(new("p3"));
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new [] { player })
                       .WithCards(new [] { 3 })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new PlayCardsCommand("g1", "p3", 3, 1);
            var handler = new PlayCardsCommandHandler(gameRepository, new InMemoryPlayerRepository(new[] { player }));
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayFirstTwoCardsShouldBeValid()
        {
            var player = new Player(new("p3"));
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCards(new [] { 3, 3 })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new PlayCardsCommand("g1", "p3", 3, 2);
            var handler = new PlayCardsCommandHandler(gameRepository, new InMemoryPlayerRepository(new[] { player }));
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerCannotPlayMoreThan4Cards()
        {
            var player = new Player(new("p1"));
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCurrentPlayer(new PlayerId("p1"))
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCurrentPlayer(new PlayerId("p1"))
                       .Build()
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new PlayCardsCommand("g1", "p1", 3, 5);
            var handler = new PlayCardsCommandHandler(gameRepository, new InMemoryPlayerRepository(new[] { player }));
            var record = await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayerCannotPlayWhenNotHisTurn()
        {
            var player = new Player(new("p3"));
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCurrentPlayer(new PlayerId("p1"))
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player })
                       .WithCurrentPlayer(new PlayerId("p1"))
                       .Build()
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new PlayCardsCommand("g1", "p3", 3, 3);
            var handler = new PlayCardsCommandHandler(gameRepository, new InMemoryPlayerRepository(new[] { player }));
            var record = await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.Equal(gameExpected, game);
        }
    }
}
