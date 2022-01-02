using President.Application.Usecases.GiveBestCards;
using President.Domain.Cards;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using President.UnitTests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class GiveBestCardsTest
    {
        [Fact]
        public async Task ShouldLoseBestCardsWhenPlayerIsAsshole()
        {
            var game = Game.FromState(new GameStateBuilder()
                                        .WithHasStarted(true)
                                        .WithPlayers(new Player[] 
                                        {
                                            Player.FromState(new PlayerStateBuilder("p1")
                                                                    .WithCards(new Card[]
                                                                    {
                                                                        new Card(12, "A", Card.Color.CLUB),
                                                                        new Card(11, "K", Card.Color.DIAMOND),
                                                                        new Card(8, "10", Card.Color.DIAMOND),
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
            var gameRepository = new InMemoryGameRepository(game);
            var command = new GiveBestCardsCommand("g1");
            var handler = new GiveBestCardsCommandHandler(gameRepository);
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }
    }
}
