using President.Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace President.Domain.Games
{
    public class Game
    {
        private readonly GameId _gameId;
        private bool _hasBegan;
        private readonly List<Player> _players;

        public Game(GameId gameId)
        {
            _gameId = gameId;
            _players = new List<Player>();
        }

        private Game(GameId gameId, bool hasBegan, Player[] players)
        {
            _gameId = gameId;
            _players = players.ToList();
            _hasBegan = hasBegan;
        }

        public static Game FromState(GameState gameState)
        {
            return new Game(new GameId(gameState.GameId), gameState.HasBegan, gameState.Players);
        }

        internal bool HasBegan() => _hasBegan;

        public GameId GameId { get => _gameId; }
        public IReadOnlyCollection<Player> Players { get => _players; }

        internal void AddPlayer(Player player)
        {
            _players.Add(player);
        }
    }
}
