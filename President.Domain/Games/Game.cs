using President.Domain.Games.Rules;
using President.Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace President.Domain.Games
{
    public class Game : Entity
    {
        private readonly GameId _gameId;
        private readonly bool _hasBegan;
        private readonly List<Player> _players;
        private readonly Dictionary<PlayerId, bool> _startRequests;

        public Game(GameId gameId)
        {
            _gameId = gameId;
            _players = new List<Player>();
            _startRequests = new Dictionary<PlayerId, bool>();
        }

        private Game(GameId gameId, bool hasBegan, Player[] players)
        {
            _gameId = gameId;
            _players = players.ToList();
            _hasBegan = hasBegan;
            _startRequests = new Dictionary<PlayerId, bool>();
        }

        internal void AcceptRequestFromPlayer(Player player)
        {
            CheckRule(new PlayerMustBeInGameRule(player, this));
            _startRequests[player.PlayerId] = true;
        }

        internal bool ContainsPlayer(Player player)
        {
            return _players.Contains(player);
        }

        public static Game FromState(GameState gameState)
        {
            return new Game(new GameId(gameState.GameId), gameState.HasBegan, gameState.Players);
        }

        internal bool HasBegan() => _hasBegan;

        public GameId GameId { get => _gameId; }
        public IReadOnlyCollection<Player> Players { get => _players; }
        public List<PlayerId> AcceptedStartRequests { get => _startRequests.Keys.ToList(); }

        internal void AddPlayer(Player player)
        {
            _players.Add(player);
            _startRequests.Add(player.PlayerId, false);
        }
    }
}
