using President.Application.Usecases.DistributeCards;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class DistributeCardsTest
    {
        [Fact]
        public async Task GameStartedShouldInitializedCards()
        {
            var p1 = new Player(new("p1"));
            var p2 = new Player(new("p2"));
            var p3 = new Player(new("p3"));
            var game = Game.FromState(new("g1", true, new Player[] { p1, p2, p3 }, Array.Empty<PlayerId>()));
            var gameRepository = new InMemoryGameRepository(game);
            var playerRepository = new InMemoryPlayerRepository(new[] { p1, p2, p3 });
            var command = new DistributeCardsCommand("g1");
            var handler = new DistributeCardsCommandHandler(gameRepository, playerRepository);
            await handler.Handle(command);
            Assert.Equal(18, p1.Cards.Length);
            Assert.Equal(17, p2.Cards.Length);
            Assert.Equal(17, p3.Cards.Length);

        }
    }
}
