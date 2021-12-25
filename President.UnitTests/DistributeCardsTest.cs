using President.Application.Usecases.DistributeCards;
using President.Domain.Cards;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static President.Domain.Cards.Card;

namespace President.UnitTests
{
    public class DistributeCardsTest
    {
        [Fact]
        public async Task GameStartedShouldDistributeCardsTo3Players()
        {
            var p1 = new Player(new("p1"));
            var p2 = new Player(new("p2"));
            var p3 = new Player(new("p3"));
            var game = Game.FromState(new("g1", true, new Player[] { p1, p2, p3 }, Array.Empty<PlayerId>()));
            var gameRepository = new InMemoryGameRepository(game);
            ICardRepository cardRepository = new InMemoryCardRepository();
            var command = new DistributeCardsCommand("g1");
            var handler = new DistributeCardsCommandHandler(gameRepository, cardRepository);
            await handler.Handle(command).ConfigureAwait(false);
            Assert.Equal(18, p1.CountCards());
            Assert.Equal(17, p2.CountCards());
            Assert.Equal(17, p3.CountCards());
        }

        [Fact]
        public async Task GameStartedShouldDistributeCardsTo5Players()
        {
            var p1 = new Player(new("p1"));
            var p2 = new Player(new("p2"));
            var p3 = new Player(new("p3"));
            var p4 = new Player(new("p4"));
            var p5 = new Player(new("p5"));
            var game = Game.FromState(new("g1", true, new Player[] { p1, p2, p3, p4, p5 }, Array.Empty<PlayerId>()));
            var gameRepository = new InMemoryGameRepository(game);
            ICardRepository cardRepository = new InMemoryCardRepository();
            var command = new DistributeCardsCommand("g1");
            var handler = new DistributeCardsCommandHandler(gameRepository, cardRepository);
            await handler.Handle(command).ConfigureAwait(false);
            Assert.Equal(11, p1.CountCards());
            Assert.Equal(11, p2.CountCards());
            Assert.Equal(10, p3.CountCards());
            Assert.Equal(10, p4.CountCards());
            Assert.Equal(10, p5.CountCards());
        }
    }
}
