using President.Domain.Games;
using President.Domain.Players;
using System.Threading.Tasks;

namespace President.Application.Usecases.PlayCards
{
    public class PlayCardsCommandHandler
    {
        private readonly IGameRepository _gameRepository;

        public PlayCardsCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(PlayCardsCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            var player = game.GetPlayer(new PlayerId(command.PlayerId));
            player.Play(command.Cards, game);
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
