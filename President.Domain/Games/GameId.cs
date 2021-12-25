namespace President.Domain.Games
{
    using President.Domain.Games.Exceptions;


    public struct GameId
    {
        private readonly string _value;

        public GameId(string gameId)
        {
            if (string.IsNullOrEmpty(gameId))
                throw new GameIdException("game id cannot be null");
            _value = gameId;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
