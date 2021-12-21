using President.Domain.Games;
using President.Domain.Players;
using System.Threading.Tasks;

namespace President.Application.Usecases.JoinGame
{
    public class JoinGameCommandHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public JoinGameCommandHandler(IGameRepository repository,
                                      IPlayerRepository playerRepository)
        {
            _gameRepository = repository;
            _playerRepository = playerRepository;
        }

        public async Task Handle(JoinGameCommand command)
        {
            var player = await _playerRepository.GetByIdAsync(new PlayerId(command.PlayerId))
                        ?? throw new System.Exception();
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            player.Join(game);
            await _gameRepository.SaveAsync(game);
        }
    }
}
