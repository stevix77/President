namespace President.Domain.Games
{
    public class Game
    {
        private GameId _gameId;

        public Game(GameId gameId)
        {
            _gameId = gameId;
        }

        public GameId GameId { get => _gameId; }
    }
}
