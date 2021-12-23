namespace President.Domain.Games
{
    using President.Domain.Games.Events;
    using President.Domain.Games.Rules;
    using President.Domain.Players;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public sealed class Game : Entity
    {
        private readonly GameId _gameId;
        private bool _hasBegan;
        private readonly List<Player> _players;
        private readonly Dictionary<PlayerId, bool> _startRequests;

        public Game(GameId gameId)
        {
            _gameId = gameId;
            _players = new List<Player>();
            _startRequests = new Dictionary<PlayerId, bool>();
            AddDomainEvent(new GameCreated(gameId));
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

        public void Start()
        {
            CheckRule(new GameMustHaveMoreTwoPlayersRule(_players.Count));
            if (HasAllPlayersRequestToStart() || IsFull())
            {
                _hasBegan = true;
                AddDomainEvent(new GameStarted(_gameId));
            }

            bool HasAllPlayersRequestToStart()
            {
                return _startRequests.All(x => x.Value);
            }

            bool IsFull()
            {
                return _players.Count == 6;
            }
        }

        internal bool IsRequestFromPlayerAccepted(Player player)
        {
            CheckRule(new PlayerMustBeInGameRule(player, this));
            CheckRule(new GameMustBeNotStartedRule(this));
            return AcceptRequestWhenNotRequestedYet(player);
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

        private bool AcceptRequestWhenNotRequestedYet(Player player)
        {
            if (!_startRequests[player.PlayerId])
            {
                _startRequests[player.PlayerId] = true;
                return true;
            }
            return false;
            
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Game);
        }

        private bool Equals(Game game)
        {
            return GetHashCode() == game.GetHashCode();
        }

        public override string ToString()
        {
            return $"{_gameId} - {_hasBegan} - {string.Join(",", _players)} - {string.Join(",", _startRequests)}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
