using President.Domain.Games;
using President.Domain.Players;
using System.Threading.Tasks;

namespace President.Application.Usecases.PlayCards
{
    public class PlayCardsCommandHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public PlayCardsCommandHandler(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public async Task Handle(PlayCardsCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            var player = await _playerRepository.GetByIdAsync(new PlayerId(command.PlayerId));
            player.Play(command.CardWeight, game, command.CountCards);
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
