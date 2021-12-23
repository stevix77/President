namespace President.Domain.Players.Events
{
    using President.Domain.Games;


    public class GameJoined : IDomainEvent
    {
        public GameJoined(PlayerId playerId, GameId gameId)
        {
            PlayerId = playerId;
            GameId = gameId;
        }

        public PlayerId PlayerId { get; }
        public GameId GameId { get; }

        public override string ToString()
        {
            return $"Player {PlayerId} joined game {GameId}";
        }
    }
}
