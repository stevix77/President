﻿using President.Domain.Cards;
using President.Domain.Players;
using System;

namespace President.UnitTests.Builders
{
    internal class PlayerStateBuilder
    {
        private readonly string _playerId;
        private int _order = 0;
        private Card[] _cards = Array.Empty<Card>();
        private bool _hasSkip;

        internal PlayerStateBuilder(string playerId)
        {
            _playerId = playerId;
        }

        internal PlayerState Build()
        {
            return new PlayerState(new PlayerId(_playerId), _order, _cards, _hasSkip);
        }

        internal PlayerStateBuilder WithOrder(int order)
        {
            _order = order;
            return this;
        }

        internal PlayerStateBuilder WithHasSkip(bool hasSkip)
        {
            _hasSkip = true;
            return this;
        }

        internal PlayerStateBuilder WithCards(Card[] cards)
        {
            _cards = cards;
            return this;
        }
    }
}
