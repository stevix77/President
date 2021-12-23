namespace President.Domain.Games
{
    using President.Domain.Exceptions;


    public struct GameId
    {
        private readonly string _value;

        public GameId(string gameId)
        {
            if (string.IsNullOrEmpty(gameId))
                throw new DomainException("game id cannot be null");
            _value = gameId;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
