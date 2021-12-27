namespace President.Domain.Games.Events
{
    using President.Domain.Players;


    class CurrentPlayerChanged : IDomainEvent
    {
        private readonly GameId _gameId;
        private readonly PlayerId _nextPlayer;

        public CurrentPlayerChanged(GameId gameId, PlayerId value)
        {
            _gameId = gameId;
            _nextPlayer = value;
        }
    }
}
