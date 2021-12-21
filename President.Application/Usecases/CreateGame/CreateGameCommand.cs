namespace President.Application.Usecases.CreateGame
{
    public class CreateGameCommand
    {
        public CreateGameCommand(string gameId)
        {
            GameId = gameId;
        }

        public string GameId { get; }
    }
}
