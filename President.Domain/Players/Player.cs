﻿namespace President.Domain.Players
{
    using President.Domain.Games;
    using President.Domain.Players.Events;
    using President.Domain.Players.Rules;
    using System.Collections.Generic;

    public class Player : Entity
    {
        private readonly PlayerId _playerId;
        private readonly List<object> _cards;
        private int _order;

        public Player(PlayerId playerId)
        {
            _playerId = playerId;
            _cards = new List<object>();
        }

        public PlayerId PlayerId { get => _playerId; }
        public int CountCards()
        {
            return _cards.Count;
        }

        internal void GetCard(object card)
        {
            _cards.Add(card);
        }

        public void RequestStartingGame(Game game)
        {
            if (game.IsRequestFromPlayerAccepted(this))
                AddDomainEvent(new StartRequested(_playerId, game.GameId));
        }

        public void Play(int cardWeight, Game game, int countCards)
        {
            CheckRule(new Play4CardsMaximumRule(countCards));
            game.AddToDeck(cardWeight, countCards, _playerId);
        }

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

        internal void SetOrder(int i)
        {
            _order = i;
        }
    }
}
