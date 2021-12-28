using President.Application.Usecases.Skip;
using President.Domain.Games;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using President.UnitTests.Builders;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class SkipTest
    {
        [Fact]
        public async Task SkipTurnShouldChangePlayer()
        {
            var expected = Game.FromState(new GameStateBuilder()
                                            .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").WithHasSkip(true).Build()),
                                                                Player.FromState(new PlayerStateBuilder("p2").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p3").Build())})
                                            .WithCurrentPlayer(new("p2"))
                                            .WithOrdering(new PlayerId[] { new("p1"), new("p2"), new("p3") })
                                            .Build());
            var actual = Game.FromState(new GameStateBuilder()
                                            .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p2").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p3").Build())})
                                            .WithCurrentPlayer(new("p1"))
                                            .WithOrdering(new PlayerId[] { new("p1"), new("p2"), new("p3") })
                                            .Build());
            var gameRepository = new InMemoryGameRepository(actual);
            var command = new SkipCommand("g1", "p1");
            var handler = new SkipCommandHandler(gameRepository);
            await handler.Handle(command);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CannotSkipWhenNotCurrentPlayer()
        {
            var expected = Game.FromState(new GameStateBuilder()
                                            .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p2").Build())})
                                            .WithCurrentPlayer(new("p2"))
                                            .WithOrdering(new PlayerId[] { new("p1"), new("p2")})
                                            .Build());
            var actual = Game.FromState(new GameStateBuilder()
                                            .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p2").Build())})
                                            .WithCurrentPlayer(new("p2"))
                                            .WithOrdering(new PlayerId[] { new("p1"), new("p2") })
                                            .Build());
            var gameRepository = new InMemoryGameRepository(actual);
            var command = new SkipCommand("g1", "p1");
            var handler = new SkipCommandHandler(gameRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task WhenStayOnePlayerTurnShouldStartsAgain()
        {
            var expected = Game.FromState(new GameStateBuilder()
                                            .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p2").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p3").Build())})
                                            .WithCurrentPlayer(new("p3"))
                                            .WithOrdering(new PlayerId[] { new("p3"), new("p2"), new("p1") })
                                            .Build());
            var actual = Game.FromState(new GameStateBuilder()
                                            .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").Build()),
                                                                Player.FromState(new PlayerStateBuilder("p2").WithHasSkip(true).Build()),
                                                                Player.FromState(new PlayerStateBuilder("p3").Build())})
                                            .WithCurrentPlayer(new("p1"))
                                            .WithOrdering(new PlayerId[] { new("p3"), new("p2"), new("p1") })
                                            .WithLastPlayer(new("p3"))
                                            .Build());
            var gameRepository = new InMemoryGameRepository(actual);
            var command = new SkipCommand("g1", "p1");
            var handler = new SkipCommandHandler(gameRepository);
            await handler.Handle(command);
            Assert.Equal(expected, actual);
        }

    }
}
