using President.Domain.Players;

namespace President.Domain.Games
{
    public class GameState
    {
        private readonly string _gameId;
        private readonly bool _hasBegan;
        private readonly Player[] _players;

        public GameState(string gameId, bool hasBegan, Player[] players)
        {
            _gameId = gameId;
            _hasBegan = hasBegan;
            _players = players;
        }

        public string GameId { get => _gameId; }
        public bool HasBegan { get => _hasBegan; }
        public Player[] Players { get => _players; }
    }
}
