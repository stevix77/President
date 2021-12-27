namespace President.UnitTests
{
    using President.Application.Usecases.JoinGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Domain.Players.Events;
    using President.Infrastructure.Repositories;
    using President.UnitTests.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class JoinGameTest
    {
        [Fact]
        public async Task PlayerShouldJoinTheGame()
        {
            var player = Player.FromState(new PlayerStateBuilder("player1").Build());
            var game = Game.FromState(new GameStateBuilder().Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                                                    .WithPlayers(new [] { player })
                                                    .Build());
            await RunHandleWillAddPlayerToGame(new InMemoryGameRepository(game),
                                               new JoinGameCommand("player1", "g1"),
                                               new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Contains(player.DomainEvents, x => x.ToString() == 
                            new GameJoined(player.PlayerId, game.GameId).ToString());
        }

        [Fact]
        public async Task GameShouldNotAddPlayerUnknown()
        {
            var game = Game.FromState(new GameStateBuilder().Build());
            var gameExpected = Game.FromState(new GameStateBuilder().Build());
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game), 
                                               new InMemoryPlayerRepository(),
                                               new JoinGameCommand(null, "g1"));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task GameBeganCannotBeJoined()
        {
            var game = Game.FromState(new GameStateBuilder()
                                        .WithHasBegan(true)
                                        .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                                        .WithHasBegan(true)
                                        .Build());
            var player = Player.FromState(new PlayerStateBuilder("player1").Build());
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game),
                                               new InMemoryPlayerRepository(new List<Player> 
                                               { 
                                                   player
                                               }),
                                               new JoinGameCommand("player1", "g1"));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        [Fact]
        public async Task GameWith6playersCannotBeJoined()
        {
            var player = Player.FromState(new PlayerStateBuilder("player1").Build());
            var players = new List<Player>(GeneratePlayers(6)) { player };
            var game = Game.FromState(new GameStateBuilder()
                                        .WithHasBegan(true)
                                        .WithPlayers(players)
                                        .Build());
            var gameExpected = Game.FromState(new GameStateBuilder()
                                        .WithHasBegan(true)
                                        .WithPlayers(players)
                                        .Build());
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game),
                                               new InMemoryPlayerRepository(players),
                                               new JoinGameCommand("player1", "g1"));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        private static IEnumerable<Player> GeneratePlayers(int nbPlayers)
        {
            for(var i = 0; i < nbPlayers; i++)
                yield return Player.FromState(new PlayerStateBuilder($"p{i+1}").Build());
        }

        private static async Task HandlerCannotAddPlayerToGame(InMemoryGameRepository gameRepository,
                                                               InMemoryPlayerRepository playerRepository,
                                                               JoinGameCommand command)
        {
            var handler = new JoinGameCommandHandler(gameRepository, playerRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
        }

        private static async Task RunHandleWillAddPlayerToGame(InMemoryGameRepository repository,
                                                               JoinGameCommand command,
                                                               InMemoryPlayerRepository playerRepository)
        {
            var handler = new JoinGameCommandHandler(repository, playerRepository);
            await handler.Handle(command);
        }
    }
}
