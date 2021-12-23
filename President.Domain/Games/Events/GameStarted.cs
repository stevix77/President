namespace President.Domain.Games.Events
{
    public class GameStarted : IDomainEvent
    {
        public GameStarted(GameId gameId)
        {
            GameId = gameId;
        }

        public GameId GameId { get; }

        public override string ToString()
        {
            return $"Game {GameId} is started";
        }
    }
}
