using President.Application.Usecases.CreateGame;
using President.Domain.Games;
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
            var gameRepository = new InMemoryGameRepository();
            await HandleWillCreateGame(new CreateGameCommand("game1"), gameRepository);
            Assert.NotNull(gameRepository.GetGame("game1"));
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
            var gameRepository = new InMemoryGameRepository(
                                    Game.FromState(new GameState("game1",
                                                                 false,
                                                                 Array.Empty<Player>(),
                                                                 new PlayerId[6])));
            await HandleWillNotCreateGame(new CreateGameCommand("game1"), gameRepository);
            Assert.Equal(1, gameRepository.CountGames());
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
