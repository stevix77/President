using President.Domain.Games;
using President.Domain.Players;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace President.Application.Usecases.DistributeCards
{
    public class DistributeCardsCommandHandler
    {
        private readonly IGameRepository gameRepository;
        private readonly IPlayerRepository playerRepository;

        public DistributeCardsCommandHandler(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            this.gameRepository = gameRepository;
            this.playerRepository = playerRepository;
        }

        public async Task Handle(DistributeCardsCommand command)
        {
            var game = await gameRepository.GetByIdAsync(new GameId("g1"));
            game.Distribute(Array.CreateInstance(typeof(object), 52).Cast<object>().ToArray());
            await gameRepository.SaveAsync(game);
        }
    }
}
