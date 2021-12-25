namespace President.UnitTests
{
    using President.Application.Usecases.JoinGame;
    using President.Domain.Games;
    using President.Domain.Players;
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
            var game = Game.FromState(new GameState("game1",
                                                    false,
                                                    Array.Empty<Player>(),
                                                    new PlayerId[6]));
            await RunHandleWillAddPlayerToGame(new InMemoryGameRepository(game),
                                               new JoinGameCommand("player1", "game1"),
                                               new InMemoryPlayerRepository(new List<Player> { new Player(new PlayerId("player1")) }));
            Assert.NotEmpty(game.Players);
        }

        [Fact]
        public async Task GameShouldNotAddPlayerUnknown()
        {
            var game = Game.FromState(new GameState("game1",
                                                    false,
                                                    Array.Empty<Player>(),
                                                    new PlayerId[6]));
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game), 
                                               new InMemoryPlayerRepository(),
                                               new JoinGameCommand(null, "game1"));
            Assert.Empty(game.Players);
        }

        [Fact]
        public async Task GameBeganCannotBeJoined()
        {
            var game = Game.FromState(new GameState("game1",
                                                    true,
                                                    Array.Empty<Player>(),
                                                    new PlayerId[6]));
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game),
                                               new InMemoryPlayerRepository(new List<Player> 
                                               { 
                                                   new Player(new PlayerId("player1")) 
                                               }),
                                               new JoinGameCommand("player1", "game1"));
            Assert.Empty(game.Players);
        }

        [Fact]
        public async Task GameWith6playersCannotBeJoined()
        {
            var players = GeneratePlayers(6);
            var game = Game.FromState(new GameState("game1",
                                                    true,
                                                    players.ToArray(),
                                                    new PlayerId[6]));
            await HandlerCannotAddPlayerToGame(new InMemoryGameRepository(game),
                                               new InMemoryPlayerRepository(players),
                                               new JoinGameCommand("player1", "game1"));
            Assert.DoesNotContain(game.Players, x => x.Equals(new Player(new PlayerId("player1"))));
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
