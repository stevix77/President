namespace President.Application.Usecases.StartGame
{
    using President.Domain.Games;
    using System.Threading.Tasks;


    public class StartGameCommandHandler
    {
        private readonly IGameRepository _gameRepository;

        public StartGameCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(StartGameCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId))
                            ?? throw new ApplicationException($"Game {command.GameId} is unknown");
            game.Start();
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
