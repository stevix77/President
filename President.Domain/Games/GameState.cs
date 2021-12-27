using President.Domain.Cards;
using President.Domain.Players;

namespace President.Domain.Games
{
    public class GameState
    {
        private readonly string _gameId;
        private readonly bool _hasBegan;
        private readonly Player[] _players;
        private readonly PlayerId[] _startingRequests;
        private readonly Card[] _cards;
        private readonly PlayerId? _currentPlayer;
        private readonly PlayerId[] _orders;
        private readonly PlayerId? _lastPlayer;

        public GameState(string gameId,
                         bool hasBegan,
                         Player[] players,
                         PlayerId[] startingRequests,
                         Card[] cards,
                         PlayerId? playerId,
                         PlayerId[] orders,
                         PlayerId? lastPlayer)
        {
            _gameId = gameId;
            _hasBegan = hasBegan;
            _players = players;
            _startingRequests = startingRequests;
            _cards = cards;
            _currentPlayer = playerId;
            _orders = orders;
            _lastPlayer = lastPlayer;
        }

        public string GameId { get => _gameId; }
        public bool HasBegan { get => _hasBegan; }
        public Player[] Players { get => _players; }
        public PlayerId[] StartingRequests { get => _startingRequests; }
        public Card[] Cards { get => _cards; }
        public PlayerId? PlayerId { get => _currentPlayer; }
        public PlayerId[] Orders { get => _orders; }
        public PlayerId? LastPlayer { get => _lastPlayer; }
    }
}
