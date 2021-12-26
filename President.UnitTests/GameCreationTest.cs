using President.Application.Usecases.CreateGame;
using President.Domain.Games;
using President.Domain.Games.Events;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class GameCreationTest
    {
        [Fact]
        public async Task ShouldCreateGame()
        {
            var expectedGame = Game.FromState(new GameState("game1",
                                                            false,
                                                            Array.Empty<Player>(),
                                                            Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3"));
            var gameRepository = new InMemoryGameRepository();
            await HandleWillCreateGame(new CreateGameCommand("game1"), gameRepository);
            Assert.Equal(expectedGame, gameRepository.GetGame("game1"));
            Assert.Contains(gameRepository.GetGame("game1").DomainEvents, 
                        x => x.ToString() == new GameCreated(new("game1")).ToString());
        }

        [Fact]
        public async Task ShouldNotCreateGameWhenIdNull()
        {
            var gameRepository = new InMemoryGameRepository();
            await HandleWillNotCreateGame(new CreateGameCommand(null), gameRepository);
            Assert.Null(gameRepository.GetGame(null));
        }

        [Fact]
        public async Task ShouldNotCreateGameWhenIdAlreadyExist()
        {
            var expectedGame = Game.FromState(new GameState("game1", false, Array.Empty<Player>(), Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3"));
            var gameRepository = new InMemoryGameRepository(
                                    Game.FromState(new GameState("game1",
                                                                 false,
                                                                 new Player[] { new Player(new("p1")) },
                                                                 Array.Empty<PlayerId>(),
                                                            Array.Empty<int>(), "p3")));
            await HandleWillNotCreateGame(new CreateGameCommand("game1"), gameRepository);
            Assert.NotEqual(expectedGame, gameRepository.GetGame("game1"));
        }

        private static async Task HandleWillNotCreateGame(CreateGameCommand command, InMemoryGameRepository gameRepository)
        {
            var handler = new CreateGameCommandHandler(gameRepository);
            await Record.ExceptionAsync(() => handler.Handle(command));
        }

        private static async Task HandleWillCreateGame(CreateGameCommand command, InMemoryGameRepository gameRepository)
        {
            var handler = new CreateGameCommandHandler(gameRepository);
            await handler.Handle(command);
        }
    }
}
