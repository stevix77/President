namespace President.UnitTests
{
    using President.Application.Usecases.JoinGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Domain.Players.Events;
    using President.Infrastructure.Repositories;
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
            var player = new Player(new("player1"));
            var game = Game.FromState(new GameState("game1",
                                                    false,
                                                    Array.Empty<Player>(),
                                                    Array.Empty<PlayerId>()));
            var gameExpected = Game.FromState(new GameState("game1",
                                                    false,
                                                    new Player[] { player },
                                                    Array.Empty<PlayerId>()));
            await RunHandleWillAddPlayerToGame(new InMemoryGameRepository(game),
                                               new JoinGameCommand("player1", "game1"),
                                               new InMemoryPlayerRepository(new List<Player> { player }));
            Assert.Equal(gameExpected, game);
            Assert.Contains(player.DomainEvents, x => x.ToString() == 
                            new GameJoined(player.PlayerId, game.GameId).ToString());
        }

        [Fact]
        public async Task GameShouldNotAddPlayerUnknown()
        {
            var game = Game.FromState(new GameState("game1",
                                                    false,
                                                    Array.Empty<Player>(),
                                                    Array.Empty<PlayerId>()));
            var gameExpected = Game.FromState(new GameState("game1",
                                                    false,
                                                    Array.Empty<Player>(),
                                                    Array.Empty<PlayerId>()));
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game), 
                                               new InMemoryPlayerRepository(),
                                               new JoinGameCommand(null, "game1"));
            Assert.Equal(gameExpected, game);
        }

        [Fact]
        public async Task GameBeganCannotBeJoined()
        {
            var game = Game.FromState(new GameState("game1",
                                                    true,
                                                    Array.Empty<Player>(),
                                                    Array.Empty<PlayerId>()));
            var gameExpected = Game.FromState(new GameState("game1",
                                                    true,
                                                    Array.Empty<Player>(),
                                                    Array.Empty<PlayerId>()));
            var player = new Player(new PlayerId("player1"));
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game),
                                               new InMemoryPlayerRepository(new List<Player> 
                                               { 
                                                   player
                                               }),
                                               new JoinGameCommand("player1", "game1"));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        [Fact]
        public async Task GameWith6playersCannotBeJoined()
        {
            var player = new Player(new("player1"));
            var players = new List<Player>(GeneratePlayers(6)) { player };
            var game = Game.FromState(new GameState("game1",
                                                    true,
                                                    players.ToArray(),
                                                    Array.Empty<PlayerId>()));
            var gameExpected = Game.FromState(new GameState("game1",
                                                    true,
                                                    players.ToArray(),
                                                    Array.Empty<PlayerId>()));
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game),
                                               new InMemoryPlayerRepository(players),
                                               new JoinGameCommand("player1", "game1"));
            Assert.Equal(gameExpected, game);
            Assert.Empty(player.DomainEvents);
        }

        private static IEnumerable<Player> GeneratePlayers(int nbPlayers)
        {
            var players = new List<Player>();
            for(var i = 0; i < nbPlayers; i++)
            {
                players.Add(new Player(new PlayerId(i.ToString())));
            }
            return players;
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
