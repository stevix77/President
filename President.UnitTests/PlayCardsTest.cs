namespace President.UnitTests
{
    using President.Application.Usecases.PlayCards;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using System;
    using System.Threading.Tasks;
    using Xunit;


    public class PlayCardsTest
    {
        [Fact]
        public async Task PlayFirstOneCardShouldBeValid()
        {
            var player = new Player(new("p1"));
            var gameExpected = Game.FromState(
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(), new int[] { 3 }, "p3")
            );
            var game = Game.FromState(
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3")
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new PlayCardsCommand("g1", "p1", 3, 1);
            var handler = new PlayCardsCommandHandler(gameRepository, new InMemoryPlayerRepository(new[] { player }));
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task PlayFirstTwoCardsShouldBeValid()
        {
            var player = new Player(new("p3"));
            var gameExpected = Game.FromState(
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(), new int[] { 3, 3 }, "p3")
            );
            var game = Game.FromState(
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3")
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
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(), Array.Empty<int>(), "p1")
            );
            var game = Game.FromState(
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p1")
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
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(), Array.Empty<int>(), "p2")
            );
            var game = Game.FromState(
                new GameState("g1", true, new Player[] { player }, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p2")
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new PlayCardsCommand("g1", "p3", 3, 3);
            var handler = new PlayCardsCommandHandler(gameRepository, new InMemoryPlayerRepository(new[] { player }));
            var record = await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.Equal(gameExpected, game);
        }
    }
}
