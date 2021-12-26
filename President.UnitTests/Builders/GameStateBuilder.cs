namespace President.UnitTests.Builders
{
    using President.Domain.Games;
    using President.Domain.Players;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class GameStateBuilder
    {
        private string _gameId;
        private bool _hasBegan;
        private Player[] _players;
        private PlayerId[] _requesters;
        private PlayerId? _currentPlayer;
        private int[]? _cardsWeight;
        private PlayerId[] _orders;

        internal GameStateBuilder()
        {
            _gameId = "g1";
            _hasBegan = false;
            _players = Array.Empty<Player>();
            _requesters = Array.Empty<PlayerId>();
            _currentPlayer = null;
            _cardsWeight = Array.Empty<int>();
            _orders = Array.Empty<PlayerId>();
        }

        internal GameStateBuilder WithCards(int[] cardsWeight)
        {
            _cardsWeight = cardsWeight;
            return this;
        }

        internal GameState Build()
        {
            return new GameState(_gameId,
                                _hasBegan,
                                _players,
                                _requesters,
                                _cardsWeight,
                                _currentPlayer,
                                _orders);
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

        internal GameStateBuilder WithHasBegan(bool hasBegan)
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
    }
}
