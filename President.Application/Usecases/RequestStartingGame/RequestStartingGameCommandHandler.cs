namespace President.Application.Usecases.RequestStartingGame
{
    using President.Domain.Games;
    using President.Domain.Players;
    using System.Threading.Tasks;


    public class RequestStartingGameCommandHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public RequestStartingGameCommandHandler(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public async Task Handle(RequestStartingGameCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId))
                            ?? throw new ApplicationException($"Game {command.GameId} is unknown");
            var player = await _playerRepository.GetByIdAsync(new PlayerId(command.PlayerId))
                            ?? throw new ApplicationException($"Player {command.PlayerId} is unknown");
            player.RequestStartingGame(game);
        }
    }
}
