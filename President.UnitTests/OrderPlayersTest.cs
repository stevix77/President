﻿using President.Application.Usecases.OrderPlayers;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure;
using President.Infrastructure.Repositories;
using President.UnitTests.Builders;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class OrderPlayersTest
    {
        [Fact]
        public async Task OrderingPlayersShouldGiveOrderToPlayer()
        {
            var game = Game.FromState(
                new GameStateBuilder()
                        .WithHasBegan(true)
                        .WithPlayers(new[] { new Player(new("p1")),
                                             new Player(new("p2")),
                                             new Player(new("p3"))})
                        .Build()
            );
            var gameExpected = Game.FromState(
                new GameStateBuilder()
                        .WithHasBegan(true)
                        .WithPlayers(new[] { new Player(new("p1")),
                                             new Player(new("p2")),
                                             new Player(new("p3"))})
                        .WithOrdering(new[] { new PlayerId("p2"), new PlayerId("p3"), new PlayerId("p1") })
                        .Build()
            );
            var gameRepository = new InMemoryGameRepository(game);
            var command = new OrderPlayersCommand("g1");
            var handler = new OrderPlayersCommandHandler(gameRepository, new InMemoryRandomProvider(2, 3, 1));
            await handler.Handle(command);
            Assert.Equal(gameExpected, game);
        }
    }
}
