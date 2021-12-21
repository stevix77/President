namespace President.Domain.Games
{
    using President.Domain.Games.Exceptions;


    public struct GameId
    {
        private readonly string _value;

        public GameId(string partyId)
        {
            if (string.IsNullOrEmpty(partyId))
                throw new GameIdException("game id cannot be null");
            _value = partyId;
        }
    }
}
