namespace President.Domain.Games.Events
{
    public class GameCreated : IDomainEvent
    {
        public GameCreated(GameId gameId)
        {
            GameId = gameId;
        }

        public GameId GameId { get; }

        public override string ToString()
        {
            return $"Game {GameId} created";
        }
    }
}
