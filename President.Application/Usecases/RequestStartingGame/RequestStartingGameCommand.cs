namespace President.Application.Usecases.RequestStartingGame
{
    public class RequestStartingGameCommand
    {
        public RequestStartingGameCommand(string playerId, string gameId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public string GameId { get; }
        public string PlayerId { get; }
    }
}
