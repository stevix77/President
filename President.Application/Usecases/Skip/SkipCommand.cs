namespace President.Application.Usecases.Skip
{
    public class SkipCommand
    {
        public SkipCommand(string gameId, string playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public string GameId { get; }
        public string PlayerId { get; }
    }
}
