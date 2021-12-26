using President.Domain.Players;
using System;

namespace President.Domain.Games
{
    public class GameState
    {
        private readonly string _gameId;
        private readonly bool _hasBegan;
        private readonly Player[] _players;
        private readonly PlayerId[] _startingRequests;
        private readonly int[] _cards;
        private readonly string _playerId;

        public GameState(string gameId,
                         bool hasBegan,
                         Player[] players,
                         PlayerId[] startingRequests,
                         int[] cards,
                         string playerId)
        {
            _gameId = gameId;
            _hasBegan = hasBegan;
            _players = players;
            _startingRequests = startingRequests;
            _cards = cards;
            _playerId = playerId;
        }

        public string GameId { get => _gameId; }
        public bool HasBegan { get => _hasBegan; }
        public Player[] Players { get => _players; }
        public PlayerId[] StartingRequests { get => _startingRequests; }
        public int[] Cards { get => _cards; }
        public PlayerId? PlayerId
        {
            get
            {
                if (string.IsNullOrEmpty(_playerId))
                    return null;
                return new PlayerId(_playerId);
            }
        }
    }
}
