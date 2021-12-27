using President.Application.Usecases.OrderPlayers;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure;
using President.Infrastructure.Repositories;
using President.UnitTests.Builders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class OrderPlayersTest
    {
        [Fact]
        public async Task OrderingPlayersShouldGiveOrderToPlayer()
        {
            var players = GeneratePlayers(3);
            var game = Game.FromState(
                new GameStateBuilder()
                        .WithHasBegan(true)
                        .WithPlayers(players)
                        .Build()
            );
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                        .WithHasBegan(true)
                        .WithPlayers(players)
                        .WithOrdering(new[] { new PlayerId("p2"), new PlayerId("p3"), new PlayerId("p1") })
                        .WithCurrentPlayer(new PlayerId("p2"))
                        .Build()
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new OrderPlayersCommand("g1");
            var handler = new OrderPlayersCommandHandler(gameRepository, new InMemoryRandomProvider(2, 3, 1));
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }

        private IEnumerable<Player> GeneratePlayers(int countPlayers)
        {
            for(var i = 1; i<= countPlayers; i++)
            {
                yield return Player.FromState(new PlayerStateBuilder($"p{i}").Build());
            }
        }
    }
}
