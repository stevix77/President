namespace President.UnitTests
{
    using President.Application.Usecases.PlayCards;
    using President.Domain.Cards;
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
            var player = Player.FromState(new PlayerStateBuilder("p3")
                                            .WithCards(new[]{ new Card(3, "5", Card.Color.CLUB),
                                                                new Card(4, "6", Card.Color.CLUB)})
                                            .WithOrder(0)
                                            .Build());
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new [] { Player.FromState(new PlayerStateBuilder("p3")
                                            .WithCards(new[]{ new Card(4, "6", Card.Color.CLUB)})
                                            .WithOrder(0)
                                            .Build()), 
                                    Player.FromState(new PlayerStateBuilder("p2")
                                                    .WithCards(new[]{ new Card(3, "5", Card.Color.CLUB),
                                                                        new Card(4, "6", Card.Color.CLUB)})
                                                    .WithOrder(1)
                                                    .Build()) })
                       .WithCards(new [] { 3 })
                       .WithCurrentPlayer(new("p2"))
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, Player.FromState(new PlayerStateBuilder("p2")
                                                                            .WithCards(new[]{ new Card(3, "5", Card.Color.CLUB),
                                                                                               new Card(4, "6", Card.Color.CLUB)})
                                                                            .WithOrder(1)
                                                                            .Build()) })
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            await AssertThatGameUpdated(player,
                                        gameExpected,
                                        game,
                                        new PlayCardsCommand("g1",
                                                             "p3",
                                                             new Card[] { new Card(3, "5", Card.Color.CLUB) }));
        }

        [Fact]
        public async Task PlayFirstTwoCardsShouldBeValid()
        {
            var player = Player.FromState(new PlayerStateBuilder("p3").Build());
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, Player.FromState(new PlayerStateBuilder("p2").Build()) })
                       .WithCards(new[] { 3, 3 })
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .WithCurrentPlayer(new("p2"))
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, Player.FromState(new PlayerStateBuilder("p2").Build()) })
                       .WithOrdering(new PlayerId[] { new("p3"), new("p2") })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            await AssertThatGameUpdated(player,
                                        gameExpected,
                                        game,
                                        new PlayCardsCommand("g1", "p3", new Card[] { new Card(3, "5", Card.Color.CLUB),
                                                                                      new Card(3, "5", Card.Color.DIAMOND)}));
        }

        [Fact]
        public async Task PlayerCannotPlayMoreThan4Cards()
        {
            var player = Player.FromState(new PlayerStateBuilder("p1").Build());
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
            await AssertThatGameDoesNotChange(player,
                                              gameExpected,
                                              game,
                                              new PlayCardsCommand("g1",
                                                                   "p1",
                                                                   new Card[] { new Card(3, "5", Card.Color.CLUB),
                                                                                new Card(), 
                                                                                new Card(),
                                                                                new Card(),
                                                                                new Card()}));
        }

        [Fact]
        public async Task PlayerCannotPlayWhenNotHisTurn()
        {
            var player = Player.FromState(new PlayerStateBuilder("p3").Build());
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
            await AssertThatGameDoesNotChange(player,
                                              gameExpected,
                                              game,
                                              new PlayCardsCommand("g1", "p3", new Card[] { new Card(3, "5", Card.Color.CLUB) }));
        }

        [Fact]
        public async Task WhenLastPlayerPlayCardShouldReturnToFirstPlayer()
        {
            var player = Player.FromState(new PlayerStateBuilder("p3").Build());
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, Player.FromState(new PlayerStateBuilder("p2").Build()) })
                       .WithCards(new[] { 3 })
                       .WithCurrentPlayer(new("p2"))
                       .WithOrdering(new PlayerId[] { new("p2"), new("p3") })
                       .Build()
            );
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithHasBegan(true)
                       .WithPlayers(new[] { player, Player.FromState(new PlayerStateBuilder("p2").Build()) })
                       .WithOrdering(new PlayerId[] { new("p2"), new("p3") })
                       .WithCurrentPlayer(new("p3"))
                       .Build()
            );
            await AssertThatGameUpdated(player,
                                        gameExpected,
                                        game,
                                        new PlayCardsCommand("g1",
                                                             "p3",
                                                             new Card[] { new Card(3, "5", Card.Color.CLUB) }));
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
