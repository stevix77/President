namespace President.UnitTests
{
    using President.Application.Usecases.StartGame;
    using President.Domain.Games;
    using President.Domain.Games.Events;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using President.UnitTests.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;


    public class StartGameTest
    {
        private readonly StartGameCommand _command;

        public StartGameTest()
        {
            _command = new StartGameCommand("g1");
        }
        [Fact]
        public async Task GameStartWhenPlayersAre6()
        {
            var players = GeneratePlayers(6);
            var game = Game.FromState(
                            new GameStateBuilder()
                       .WithPlayers(players)
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithHasStarted(true)
                       .WithPlayers(players)
                       .Build());
            await HandleStartTheGame(new InMemoryGameRepository(game));
            Assert.Equal(gameExpected, game);
            Assert.Contains(game.DomainEvents, x => x.ToString() == new GameStarted(new("g1")).ToString());
        }

        [Fact]
        public async Task GameNotStartWhenPlayersAreLessThan3()
        {
            var players = GeneratePlayers(2);
            var game = Game.FromState(new GameStateBuilder()
                       .WithPlayers(players)
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithPlayers(players)
                       .Build());
            await HandleWillNotStartTheGame(new InMemoryGameRepository(game));
            Assert.Equal(gameExpected, game);
            Assert.Empty(game.DomainEvents);
        }

        [Fact]
        public async Task GameHaveToExistsToStart()
        {
            Exception record = await HandleWillNotStartTheGame(new InMemoryGameRepository());
            Assert.NotNull(record);
        }

        [Fact]
        public async Task GameCanStartWhenAllPlayersRequestsToStart()
        {
            var players = GeneratePlayers(3);
            var game = Game.FromState(new GameStateBuilder()
                       .WithPlayers(players)
                       .WithRequesters(players.Select(x => x.PlayerId))
                       .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                       .WithHasStarted(true)
                       .WithPlayers(players)
                       .WithRequesters(players.Select(x => x.PlayerId))
                       .Build());
            await HandleStartTheGame(new InMemoryGameRepository(game));
            Assert.Equal(gameExpected, game);
            Assert.Contains(game.DomainEvents, x => x.ToString() == new GameStarted(new("g1")).ToString());
        }

        [Fact]
        public async Task GameCannotStartWhenAnyPlayerRequestsToStart()
        {
            var players = GeneratePlayers(3);
            var game = Game.FromState(
                new GameStateBuilder()
                       .WithPlayers(players)
                       .WithRequesters(players.Take(2).Select(x => x.PlayerId))
                       .Build());
            var gameExpected = Game.FromState(
                            new GameStateBuilder()
                                .WithPlayers(players)
                                .WithRequesters(players.Take(2).Select(x => x.PlayerId))
                                .Build());
            await HandleStartTheGame(new InMemoryGameRepository(game));
            Assert.Equal(gameExpected, game);
            Assert.Empty(game.DomainEvents);
        }

        private async Task<Exception> HandleWillNotStartTheGame(InMemoryGameRepository gameRepository)
        {
            var handler = new StartGameCommandHandler(gameRepository);
            var record = await Record.ExceptionAsync(() => handler.Handle(_command));
            return record;
        }

        private async Task HandleStartTheGame(InMemoryGameRepository gameRepository)
        {
            var handler = new StartGameCommandHandler(gameRepository);
            await handler.Handle(_command);
        }

        private IEnumerable<Player> GeneratePlayers(int nbPlayers)
        {
            for (var i = 0; i < nbPlayers; i++)
                yield return Player.FromState(new PlayerStateBuilder($"p{i + 1}").Build());
        }
    }
}
