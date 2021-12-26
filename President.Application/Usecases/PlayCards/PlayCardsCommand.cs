namespace President.Application.Usecases.PlayCards
{
    public class PlayCardsCommand
    {
        public PlayCardsCommand(string gameId, string playerId, int cardWeight, int v)
        {
            GameId = gameId;
            PlayerId = playerId;
            CardWeight = cardWeight;
            CountCards = v;
        }

        public string GameId { get; }
        public string PlayerId { get; }
        public int CardWeight { get; }
        public int CountCards { get; }
    }
}
