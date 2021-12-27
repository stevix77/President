namespace President.Domain.Players
{
    using President.Domain.Cards;
    using President.Domain.Games;
    using President.Domain.Players.Events;
    using President.Domain.Players.Rules;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Player : Entity
    {
        private readonly PlayerId _playerId;
        private readonly List<Card> _cards;
        private int _order;

        private Player(PlayerId playerId, int order, Card[] cards)
        {
            _playerId = playerId;
            _cards = cards.ToList();
            _order = order;
        }

        public static Player FromState(PlayerState state)
        {
            return new Player(state.PlayerId,
                              state.Order,
                              state.Cards);
        }

        public PlayerId PlayerId { get => _playerId; }
        public int CountCards()
        {
            return _cards.Count;
        }

        internal void GetCard(Card card)
        {
            _cards.Add(card);
        }

        public void RequestStartingGame(Game game)
        {
            if (game.IsRequestFromPlayerAccepted(this))
                AddDomainEvent(new StartRequested(_playerId, game.GameId));
        }

        public void Play(IEnumerable<Card> cards, Game game)
        {
            CheckRule(new Play4CardsMaximumRule(cards.Count()));
            if (cards.All(x => _cards.Contains(x)))
            {
                game.AddToDeck(cards, _playerId);
                DropCards(cards);
            }
        }

        private void DropCards(IEnumerable<Card> cards)
        {
            _cards.RemoveAll(x => cards.Any(y => y.Equals(x)));
        }

        public void Join(Game game)
        {
            game.AddPlayer(this);
            AddDomainEvent(new GameJoined(_playerId, game.GameId));
        }

        internal void SetOrder(int i)
        {
            _order = i;
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return $"{_playerId} - order {_order} - cards {string.Join(",", _cards)}";
        }
    }
}
