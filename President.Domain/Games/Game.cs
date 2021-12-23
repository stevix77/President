using President.Domain.Games.Rules;
using President.Domain.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace President.Domain.Games
{
    public sealed class Game : Entity
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

        private Game(GameId gameId,
                     bool hasBegan,
                     Player[] players,
                     PlayerId[] startingRequests)
        {
            _gameId = gameId;
            _players = players.ToList();
            _hasBegan = hasBegan;
            _startRequests = new Dictionary<PlayerId, bool>(
                                    players.Select(x => new KeyValuePair<PlayerId, bool>(
                                                    x.PlayerId, startingRequests.Contains(x.PlayerId))
                                                   )
                                    );
        }

        public static Game FromState(GameState gameState)
        {
            return new Game(new GameId(gameState.GameId),
                            gameState.HasBegan,
                            gameState.Players,
                            gameState.StartingRequests);
        }

        public GameId GameId { get => _gameId; }

        internal bool HasBegan() => _hasBegan;
        internal void AcceptRequestFromPlayer(Player player)
        {
            CheckRule(new PlayerMustBeInGameRule(player, this));
            CheckRule(new GameMustBeNotStartedRule(this));
            AcceptRequestWhenNotRequestedYet(player);

        }

        internal bool ContainsPlayer(Player player)
        {
            return _players.Contains(player);
        }

        internal void AddPlayer(Player player)
        {
            CheckRule(new GameMustBeNotStartedRule(this));
            _players.Add(player);
            _startRequests.Add(player.PlayerId, false);
        }

        private void AcceptRequestWhenNotRequestedYet(Player player)
        {
            if (!_startRequests[player.PlayerId])
                _startRequests[player.PlayerId] = true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Game);
        }

        private bool Equals(Game game)
        {
            return EqualityComparer<GameId>.Default.Equals(_gameId, game._gameId) &&
                   _hasBegan == game._hasBegan &&
                   Enumerable.SequenceEqual(_players, game._players) &&
                   Enumerable.SequenceEqual(_startRequests, game._startRequests);
        }
    }
}
