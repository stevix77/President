namespace President.Domain.Games
{
    using President.Domain.Cards;
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
        private PlayerId? _currentPlayer;
        private readonly List<Player> _players;
        private readonly Dictionary<PlayerId, bool> _startRequests;
        private readonly List<int> _cards;

        public Game(GameId gameId)
        {
            _gameId = gameId;
            _players = new List<Player>();
            _startRequests = new Dictionary<PlayerId, bool>();
            _cards = new List<int>();
            AddDomainEvent(new GameCreated(gameId));
        }

        private Game(GameId gameId,
                     bool hasBegan,
                     Player[] players,
                     PlayerId[] startingRequests,
                     int[] cards,
                     PlayerId? playerId)
        {
            _gameId = gameId;
            _players = players.ToList();
            _hasBegan = hasBegan;
            _startRequests = new Dictionary<PlayerId, bool>(
                                    players.Select(x => new KeyValuePair<PlayerId, bool>(
                                                    x.PlayerId, startingRequests.Contains(x.PlayerId))
                                                   )
                                    );
            _cards = cards.ToList();
            _currentPlayer = playerId;
        }

        internal void AddToDeck(int cardWeight, int countCards, PlayerId playerId)
        {
            if (playerId.Equals(_currentPlayer))
            {
                for (var i = 0; i < countCards; i++)
                    _cards.Add(cardWeight);
            }
        }

        public static Game FromState(GameState gameState)
        {
            return new Game(new GameId(gameState.GameId),
                            gameState.HasBegan,
                            gameState.Players,
                            gameState.StartingRequests,
                            gameState.Cards,
                            gameState.PlayerId);
        }

        public void Distribute(Card[] cards)
        {
            foreach(var card in cards)
            {
                GiveCard(card, GetPlayerWithLessCards());
            }
        }

        private void GiveCard(object card, Player player)
        {
            player.GetCard(card);
        }

        private Player GetPlayerWithLessCards()
        {
            var min = _players.Min(x => x.CountCards());
            return _players.FirstOrDefault(x => x.CountCards() == min);
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
            return $"{_gameId} - {_hasBegan} - {string.Join(",", _players)} - {string.Join(",", _startRequests)}" +
                $" - {string.Join(",", _cards)}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
