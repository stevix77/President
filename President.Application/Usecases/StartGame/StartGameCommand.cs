namespace President.Application.Usecases.StartGame
{
    public class StartGameCommand
    {
        public StartGameCommand(string gameId)
        {
            GameId = gameId;
        }

        public string GameId { get; }
    }
}
