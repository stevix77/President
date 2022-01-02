using President.Application.Usecases.GiveCards;
using President.Domain.Cards;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using President.UnitTests.Builders;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class GiveCardsTest
    {
        [Fact]
        public async Task PresidentShouldGiveTwoCardsToAsshole()
        {
            var game = Game.FromState(new GameStateBuilder()
                                        .WithHasStarted(true)
                                        .WithPlayers(new Player[]
                                        {
                                            Player.FromState(new PlayerStateBuilder("p1")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(8, "10", Card.Color.DIAMOND)
                                                                    })
                                                                    .WithRank(3)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p2")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(13, "2", Card.Color.CLUB),
                                                                        new Card(10, "Q", Card.Color.DIAMOND),
                                                                        new Card(7, "9", Card.Color.DIAMOND),
                                                                    })
                                                                    .WithRank(2)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p3")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(12, "A", Card.Color.SPADE),
                                                                        new Card(11, "K", Card.Color.HEART),
                                                                        new Card(6, "8", Card.Color.DIAMOND),
                                                                        new Card(12, "A", Card.Color.CLUB),
                                                                        new Card(11, "K", Card.Color.DIAMOND)
                                                                    })
                                                                    .WithRank(1)
                                                                    .Build()),
                                        })
                                                  .Build());

            var gameExpected = Game.FromState(new GameStateBuilder()
                                        .WithHasStarted(true)
                                        .WithPlayers(new Player[]
                                        {
                                            Player.FromState(new PlayerStateBuilder("p1")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(8, "10", Card.Color.DIAMOND),
                                                                        new Card(11, "K", Card.Color.HEART),
                                                                        new Card(6, "8", Card.Color.DIAMOND)
                                                                    })
                                                                    .WithRank(3)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p2")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(13, "2", Card.Color.CLUB),
                                                                        new Card(10, "Q", Card.Color.DIAMOND),
                                                                        new Card(7, "9", Card.Color.DIAMOND),
                                                                    })
                                                                    .WithRank(2)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p3")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(12, "A", Card.Color.SPADE),
                                                                        new Card(12, "A", Card.Color.CLUB),
                                                                        new Card(11, "K", Card.Color.DIAMOND)
                                                                    })
                                                                    .WithRank(1)
                                                                    .Build()),
                                        })
                                                  .Build());
            var gameRepository = new InMemoryGameRepository(game);
            var command = new GiveCardsCommand("g1", new Card[] { new Card(11, "K", Card.Color.HEART),
                                                                        new Card(6, "8", Card.Color.DIAMOND) });
            var handler = new GiveCardsCommandHandler(gameRepository);
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task VicePresidentShouldGiveOneCardToViceAsshole()
        {
            var game = Game.FromState(new GameStateBuilder()
                                        .WithHasStarted(true)
                                        .WithPlayers(new Player[]
                                        {
                                            Player.FromState(new PlayerStateBuilder("p1")
                                                                    .WithRank(3)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p2")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(13, "2", Card.Color.CLUB),
                                                                        new Card(10, "Q", Card.Color.DIAMOND),
                                                                        new Card(7, "9", Card.Color.DIAMOND)
                                                                    })
                                                                    .WithRank(2)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p3")
                                                                    .WithRank(1)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p4")
                                                                    .WithRank(4)
                                                                    .Build())
                                        })
                                                  .Build());

            var gameExpected = Game.FromState(new GameStateBuilder()
                                        .WithHasStarted(true)
                                        .WithPlayers(new Player[]
                                        {
                                            Player.FromState(new PlayerStateBuilder("p1")
                                                                    .WithRank(3)
                                                                    .WithCards(new Card[]{ new Card(7, "9", Card.Color.DIAMOND) })
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p2")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(13, "2", Card.Color.CLUB),
                                                                        new Card(10, "Q", Card.Color.DIAMOND)
                                                                    })
                                                                    .WithRank(2)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p3")
                                                                    .WithRank(1)
                                                                    .Build()),
                                            Player.FromState(new PlayerStateBuilder("p4")
                                                                    .WithRank(4)
                                                                    .Build())
                                        })
                                                  .Build());
            var gameRepository = new InMemoryGameRepository(game);
            var command = new GiveCardsCommand("g1", new Card[] { new Card(7, "9", Card.Color.DIAMOND) });
            var handler = new GiveCardsCommandHandler(gameRepository);
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }
    }
}
