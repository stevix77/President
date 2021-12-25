namespace President.UnitTests
{
    using President.Application.Usecases.JoinGame;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using System.Threading.Tasks;
    using Xunit;

    public class JoinGameTest
    {
        [Fact]
        public async Task PlayerShouldJoinTheGame()
        {
            var game = new Game(new GameId("game1"));
            await RunHandleWillAddPlayerToGame(new InMemoryGameRepository(game), new JoinGameCommand("player1", "game1"));
            Assert.NotEmpty(game.Players);
        }

        [Fact]
        public async Task GameShouldNotAddPlayerUnknown()
        {
            var game = new Game(new GameId("game1"));
            await HandlerCannotAddPlayerToGame(game);
            Assert.Empty(game.Players);
        }

        private static async Task HandlerCannotAddPlayerToGame(Game game)
        {
            var handler = new JoinGameCommandHandler(new InMemoryGameRepository(game), new InMemoryPlayerRepository());
            await Record.ExceptionAsync(() => handler.Handle(new JoinGameCommand("", "game1")));
        }

        private static async Task RunHandleWillAddPlayerToGame(InMemoryGameRepository repository, JoinGameCommand command)
        {
            var handler = new JoinGameCommandHandler(repository, new InMemoryPlayerRepository(new Player(new PlayerId("player1"))));
            await handler.Handle(command);
        }
    }
}
