using President.Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace President.Domain.Games
{
    public class GameState
    {
        private readonly string _gameId;
        private readonly bool _hasBegan;
        private readonly Player[] _players;
        private readonly PlayerId[] _startingRequests;
        private readonly int[] _cards;
        private readonly PlayerId? _currentPlayer;
        private readonly PlayerId[] _orders;

        public GameState(string gameId,
                         bool hasBegan,
                         Player[] players,
                         PlayerId[] startingRequests,
                         int[] cards,
                         PlayerId? playerId,
                         PlayerId[] orders)
        {
            _gameId = gameId;
            _hasBegan = hasBegan;
            _players = players;
            _startingRequests = startingRequests;
            _cards = cards;
            _currentPlayer = playerId;
            _orders = orders;
        }

        public string GameId { get => _gameId; }
        public bool HasBegan { get => _hasBegan; }
        public Player[] Players { get => _players; }
        public PlayerId[] StartingRequests { get => _startingRequests; }
        public int[] Cards { get => _cards; }
        public PlayerId? PlayerId { get => _currentPlayer; }
        public PlayerId[] Orders { get => _orders; }
    }
}
