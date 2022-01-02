using President.Domain.Games;
using System.Linq;
using System.Threading.Tasks;

namespace President.Application.Usecases.GiveCards
{
    public class GiveCardsCommandHandler
    {
        private const int RANK_PRESIDENT = 1;
        private const int RANK_VICE_PRESIDENT = 2;
        private const int COUNT_CARDS_GIVEN_BY_PRESIDENT = 2;
        private readonly IGameRepository _gameRepository;

        public GiveCardsCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(GiveCardsCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            var player = game.GetPlayerAtRank(command.Cards.Count() == COUNT_CARDS_GIVEN_BY_PRESIDENT ? RANK_PRESIDENT : RANK_VICE_PRESIDENT);
            var playerToGiveCards = game.GetPlayerAtRank(command.Cards.Count() == COUNT_CARDS_GIVEN_BY_PRESIDENT 
                                        ? game.CountPlayer() 
                                        : game.CountPlayer() - 1);
            player.GiveCards(command.Cards, playerToGiveCards);
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
