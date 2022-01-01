using President.Domain.Games;
using System.Threading.Tasks;

namespace President.Application.Usecases.GiveBestCards
{
    public class GiveBestCardsCommandHandler
    {
        private readonly IGameRepository _gameRepository;

        public GiveBestCardsCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(GiveBestCardsCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            var asshole = game.GetAsshole();
            asshole.GiveBestCards(game);

            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
