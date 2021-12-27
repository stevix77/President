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
                       .WithPlayers(new [] { player, new Player(new("p2")) })
                       .WithCards(new [] { 3 })
                       .WithCurrentPlayer(new("p2"))
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, new Player(new("p2")) })
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            await AssertThatGameUpdated(player, gameExpected, game, new PlayCardsCommand("g1", "p3", 3, 1));
        }

        [Fact]
        public async Task PlayFirstTwoCardsShouldBeValid()
        {
            var player = new Player(new("p3"));
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, new Player(new("p2")) })
                       .WithCards(new[] { 3, 3 })
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .WithCurrentPlayer(new("p2"))
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, new Player(new("p2")) })
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            await AssertThatGameUpdated(player, gameExpected, game, new PlayCardsCommand("g1", "p3", 3, 2));
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
            await AssertThatGameDoesNotChange(player, gameExpected, game, new PlayCardsCommand("g1", "p1", 3, 5));
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
            await AssertThatGameDoesNotChange(player, gameExpected, game, new PlayCardsCommand("g1", "p3", 3, 3));
        }

        [Fact]
        public async Task WhenLastPlayerPlayCardShouldReturnToFirstPlayer()
        {
            var player = new Player(new("p3"));
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, new Player(new("p2")) })
                       .WithCards(new[] { 3 })
                       .WithCurrentPlayer(new("p2"))
                       .WithOrdering(new PlayerId[] { new("p2"), new("p3") })
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, new Player(new("p2")) })
                       .WithOrdering(new PlayerId[] { new("p2"), new("p3") })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            await AssertThatGameUpdated(player, gameExpected, game, new PlayCardsCommand("g1", "p3", 3, 1));
        }

        private static async Task AssertThatGameDoesNotChange(Player player, Game gameExpected, Game game, PlayCardsCommand command)
        {
            var handler = new PlayCardsCommandHandler(new InMemoryGameRepository(game), new InMemoryPlayerRepository(new[] { player }));
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.Equal(gameExpected, game);
        }

        private static async Task AssertThatGameUpdated(Player player, Game gameExpected, Game game, PlayCardsCommand command)
        {
            var handler = new PlayCardsCommandHandler(new InMemoryGameRepository(game),
                                                      new InMemoryPlayerRepository(new[] { player }));
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }
    }
}
