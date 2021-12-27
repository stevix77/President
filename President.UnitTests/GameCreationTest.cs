using President.Application.Usecases.CreateGame;
using President.Domain.Games;
using President.Domain.Games.Events;
using President.Domain.Players;
using President.Infrastructure.Repositories;
using President.UnitTests.Builders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class GameCreationTest
    {
        private const string _gameId = "g1";

        [Fact]
        public async Task ShouldCreateGame()
        {
            var expectedGame = Game.FromState(new GameStateBuilder().Build());
            var gameRepository = new InMemoryGameRepository();
            await HandleWillCreateGame(new CreateGameCommand(_gameId), gameRepository);
            Assert.Equal(expectedGame, gameRepository.GetGame(_gameId));
            Assert.Contains(gameRepository.GetGame(_gameId).DomainEvents, 
                        x => x.ToString() == new GameCreated(new(_gameId)).ToString());
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
            var expectedGame = Game.FromState(new GameStateBuilder().Build());
            var gameRepository = new InMemoryGameRepository(
                                    Game.FromState(new GameStateBuilder()
                                                        .WithPlayers(new[] { Player.FromState(new PlayerStateBuilder("p1").Build()) })
                                                        .Build()));
            await HandleWillNotCreateGame(new CreateGameCommand(_gameId), gameRepository);
            Assert.NotEqual(expectedGame, gameRepository.GetGame(_gameId));
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
