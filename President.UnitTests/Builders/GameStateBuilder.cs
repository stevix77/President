namespace President.UnitTests.Builders
{
    using President.Domain.Cards;
    using President.Domain.Games;
    using President.Domain.Players;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class GameStateBuilder
    {
        private readonly string _gameId;
        private bool _hasBegan;
        private Player[] _players;
        private PlayerId[] _requesters;
        private PlayerId? _currentPlayer;
        private Card[] _cards;
        private PlayerId[] _orders;
        private PlayerId? _lastPlayer;

        internal GameStateBuilder()
        {
            _gameId = "g1";
            _hasBegan = false;
            _players = Array.Empty<Player>();
            _requesters = Array.Empty<PlayerId>();
            _currentPlayer = null;
            _cards = Array.Empty<Card>();
            _orders = Array.Empty<PlayerId>();
        }

        internal GameStateBuilder WithCards(Card[] cards)
        {
            _cards = cards;
            return this;
        }

        internal GameState Build()
        {
            return new GameState(_gameId,
                                _hasBegan,
                                _players,
                                _requesters,
                                _cards,
                                _currentPlayer,
                                _orders,
                                _lastPlayer);
        }

        internal GameStateBuilder WithOrdering(PlayerId[] playerIds)
        {
            _orders = playerIds;
            return this;
        }

        internal GameStateBuilder WithPlayers(IEnumerable<Player> players)
        {
            _players = players.ToArray();
            return this;
        }

        internal GameStateBuilder WithHasStarted(bool hasBegan)
        {
            _hasBegan = hasBegan;
            return this;
        }

        internal GameStateBuilder WithCurrentPlayer(PlayerId playerId)
        {
            _currentPlayer = playerId;
            return this;
        }

        internal GameStateBuilder WithRequesters(IEnumerable<PlayerId> requesters)
        {
            _requesters = requesters.ToArray();
            return this;
        }

        internal GameStateBuilder WithLastPlayer(PlayerId playerId)
        {
            _lastPlayer = playerId;
            return this;
        }
    }
}
