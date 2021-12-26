namespace President.UnitTests
{
    using President.Application.Usecases.StartGame;
    using President.Domain.Games;
    using President.Domain.Games.Events;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
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
                            new GameState("g1", false, players, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3"));
            var gameExpected = Game.FromState(
                            new GameState("g1", true, players, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3"));
            await HandleStartTheGame(new InMemoryGameRepository(game));
            Assert.Equal(gameExpected, game);
            Assert.Contains(game.DomainEvents, x => x.ToString() == new GameStarted(new("g1")).ToString());
        }

        [Fact]
        public async Task GameNotStartWhenPlayersAreLessThan3()
        {
            var players = GeneratePlayers(2);
            var game = Game.FromState(
                            new GameState("g1", false, players, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3"));
            var gameExpected = Game.FromState(
                            new GameState("g1", false, players, Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3"));
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
            var game = Game.FromState(
                            new GameState("g1", false, players, players.Select(x => x.PlayerId).ToArray(),
                                                            Array.Empty<int>(), "p3"));
            var gameExpected = Game.FromState(
                            new GameState("g1", true, players, players.Select(x => x.PlayerId).ToArray(),
                                                            Array.Empty<int>(), "p3"));
            await HandleStartTheGame(new InMemoryGameRepository(game));
            Assert.Equal(gameExpected, game);
            Assert.Contains(game.DomainEvents, x => x.ToString() == new GameStarted(new("g1")).ToString());
        }

        [Fact]
        public async Task GameCannotStartWhenAnyPlayerRequestsToStart()
        {
            var players = GeneratePlayers(3);
            var game = Game.FromState(
                            new GameState("g1", false, players, players.Take(2).Select(x => x.PlayerId).ToArray(),
                                                            Array.Empty<int>(), "p3"));
            var gameExpected = Game.FromState(
                            new GameState("g1", false, players, players.Take(2).Select(x => x.PlayerId).ToArray(),
                                                            Array.Empty<int>(), "p3"));
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

        private Player[] GeneratePlayers(int nbPlayers)
        {
            var players = new List<Player>();
            for (var i = 0; i < nbPlayers; i++)
            {
                players.Add(new Player(new(i.ToString())));
            }
            return players.ToArray();
        }
    }
}
