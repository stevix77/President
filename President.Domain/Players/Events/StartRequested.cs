namespace President.Domain.Players.Events
{
    using President.Domain.Games;

    public class StartRequested : IDomainEvent
    {
        public StartRequested(PlayerId playerId, GameId gameId)
        {
            PlayerId = playerId;
            GameId = gameId;
        }

        public PlayerId PlayerId { get; }
        public GameId GameId { get; }

        public override string ToString()
        {
            return $"Player {PlayerId} has requested to start game {GameId}";
        }
    }
}
