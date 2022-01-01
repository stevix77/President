using President.Domain.Games;
using System;
using System.Threading.Tasks;

namespace President.Application.Usecases.GiveCards
{
    public class GiveCardsCommandHandler
    {
        private readonly IGameRepository _gameRepository;

        public GiveCardsCommandHandler(IGameRepository gameRepository)
        {
            this._gameRepository = gameRepository;
        }

        public async Task Handle(GiveCardsCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            var president = game.GetPresident();
            president.GiveCards(command.Cards, game.GetAsshole());
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
