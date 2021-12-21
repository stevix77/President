namespace President.Application.Usecases.JoinGame
{
    public class JoinGameCommand
    {
        public JoinGameCommand(string playerId, string gameId)
        {
            PlayerId = playerId;
            GameId = gameId;
        }

        public string GameId { get; }
        public string PlayerId { get; }
    }
}
