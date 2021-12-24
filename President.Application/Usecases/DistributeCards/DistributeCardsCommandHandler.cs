using President.Domain.Cards;
using President.Domain.Games;
using System.Threading.Tasks;

namespace President.Application.Usecases.DistributeCards
{
    public class DistributeCardsCommandHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICardRepository _cardRepository;

        public DistributeCardsCommandHandler(IGameRepository gameRepository, ICardRepository cardRepository)
        {
            _gameRepository = gameRepository;
            _cardRepository = cardRepository;
        }

        public async Task Handle(DistributeCardsCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            var cards = await _cardRepository.GetCards().ConfigureAwait(false);
            game.Distribute(cards);
            await _gameRepository.SaveAsync(game);
        }
    }
}
