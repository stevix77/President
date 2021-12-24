using President.Domain.Games;

namespace President.Application.Usecases.DistributeCards
{
    public class DistributeCardsCommand
    {
        public DistributeCardsCommand(string gameId)
        {
            GameId = gameId;
        }

        public string GameId { get; }
    }
}
