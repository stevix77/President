using President.Domain.Games;

namespace President.Application.Usecases.OrderPlayers
{
    public class OrderPlayersCommand
    {
        public OrderPlayersCommand(string gameId)
        {
            GameId = gameId;
        }

        public string GameId { get; }
    }
}
