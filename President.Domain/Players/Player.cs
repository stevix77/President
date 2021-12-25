namespace President.Domain.Players
{
    using President.Domain.Games;
    using President.Domain.Players.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Player : Entity
    {
        private readonly PlayerId _playerId;
        private readonly List<object> _cards;

        public Player(PlayerId playerId)
        {
            _playerId = playerId;
            _cards = new List<object>();
        }

        public PlayerId PlayerId { get => _playerId; }
        public object[] Cards { get => _cards.ToArray(); }

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

        internal void GetCard(object card)
        {
            _cards.Add(card);
        }
    }
}
