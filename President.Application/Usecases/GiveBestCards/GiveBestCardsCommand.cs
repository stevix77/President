namespace President.Application.Usecases.GiveBestCards
{
    public class GiveBestCardsCommand
    {
        public GiveBestCardsCommand(string gameId)
        {
            GameId = gameId;
        }

        public string GameId { get; }
    }
}
