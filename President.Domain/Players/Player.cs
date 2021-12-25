namespace President.Domain.Players
{
    using President.Domain.Games;
    using President.Domain.Players.Events;

    public class Player : Entity
    {
        private readonly PlayerId _playerId;

        public Player(PlayerId playerId)
        {
            _playerId = playerId;
        }

        public PlayerId PlayerId { get => _playerId; }

        public void Join(Game game)
        {
            game.AddPlayer(this);
            AddDomainEvent(new GameJoined(_playerId, game.GameId));
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return _playerId.ToString();
        }

        public void RequestStartingGame(Game game)
        {
            if (game.IsRequestFromPlayerAccepted(this))
                AddDomainEvent(new StartRequested(_playerId, game.GameId));
        }
    }
}
