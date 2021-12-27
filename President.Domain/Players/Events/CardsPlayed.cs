using President.Domain.Games;

namespace President.Domain.Players.Events
{
    class CardsPlayed : IDomainEvent
    {
        private readonly PlayerId _playerId;
        private readonly GameId _gameId;

        public CardsPlayed(PlayerId playerId, GameId gameId)
        {
            _playerId = playerId;
            _gameId = gameId;
        }
    }
}
