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
        private const int MAX_PLAYERS_AUTHORIZED = 6;
        private readonly GameId _gameId;
        private bool _hasStarted;
        private PlayerId? _currentPlayer;
        private PlayerId? _lastPlayer;
        private readonly Dictionary<int, PlayerId> _orders;
        private readonly List<Player> _players;
        private Dictionary<PlayerId, bool> _startRequests;
        private readonly List<Card> _cards;

        public Game(GameId gameId)
        {
            _gameId = gameId;
            _players = new List<Player>();
            _startRequests = new Dictionary<PlayerId, bool>();
            _cards = new List<Card>();
            _orders = new Dictionary<int, PlayerId>();
            AddDomainEvent(new GameCreated(gameId));
        }

        private Game(GameId gameId,
                     bool hasBegan,
                     Player[] players,
                     PlayerId[] startingRequests,
                     Card[] cards,
                     PlayerId? playerId,
                     PlayerId[] orders,
                     PlayerId? lastPlayer)
        {
            _gameId = gameId;
            _players = players.ToList();
            _hasStarted = hasBegan;
            _startRequests = new Dictionary<PlayerId, bool>(
                                    players.Select(x => new KeyValuePair<PlayerId, bool>(
                                                    x.PlayerId, startingRequests.Contains(x.PlayerId))
                                                   )
                                    );
            _cards = cards.ToList();
            _currentPlayer = playerId;
            _orders = new Dictionary<int, PlayerId>(
                        orders.Select(x => new KeyValuePair<int, PlayerId>(
                            orders.ToList().IndexOf(x), x)));
            _lastPlayer = lastPlayer;
        }

        public static Game FromState(GameState gameState)
        {
            return new Game(new GameId(gameState.GameId),
                            gameState.HasBegan,
                            gameState.Players,
                            gameState.StartingRequests,
                            gameState.Cards,
                            gameState.PlayerId,
                            gameState.Orders,
                            gameState.LastPlayer);
        }

        public int CountPlayer() => _players.Count;

        public Player GetAsshole()
        {
            return _players.First(x => x.IsAsshole(_players.Count));
        }

        public Player GetPlayerAtRank(int rank)
        {
            return _players.Find(x => x.Rank == rank);
        }

        public void OrderPlayers(IRandomNumberProvider randomNumberProvider)
        {
            if (!IsOrderingDone())
            {
                for (var i = 0; i < _players.Count; i++)
                {
                    var number = randomNumberProvider.GetNextNumber(1, _players.Count);
                    var player = _players.ElementAt(number - 1);
                    _orders.Add(i, player.PlayerId);
                    player.SetOrder(i);
                }
                _currentPlayer = _orders[0];
                return;
            }
            _currentPlayer = GetAsshole().PlayerId;
            AddDomainEvent(new CurrentPlayerChanged(_gameId, _currentPlayer.Value));
        }

        public void Distribute(Card[] cards)
        {
            foreach(var card in cards)
            {
                GiveCard(card, GetPlayerWithLessCards());
            }
            AddDomainEvent(new CardsDistributed(_gameId));
        }

        public void GiveBestCardsToFirstPlayers()
        {
            var asshole = GetAsshole();
            asshole.GiveBestCards(2, GetPlayerAtRank(1));
            if (CountPlayer() > 3)
            {
                var viceAsshole = GetViceAsshole();
                viceAsshole.GiveBestCards(1, GetPlayerAtRank(2));
            }
        }

        public Player GetPlayer(PlayerId playerId)
        {
            return _players.FirstOrDefault(x => x.PlayerId.Equals(playerId));
        }

        public GameId GameId { get => _gameId; }
        public PlayerId CurrentPlayer { get => _currentPlayer.GetValueOrDefault(); }

        public void Start()
        {
            CheckRule(new GameMustHaveMoreTwoPlayersRule(_players.Count));
            if (HasAllPlayersRequestToStart() || IsFull())
            {
                _hasStarted = true;
                AddDomainEvent(new GameStarted(_gameId));
            }
        }

        internal bool HasBegan() => _hasStarted;
        internal bool IsRequestFromPlayerAccepted(Player player)
        {
            CheckRule(new PlayerMustBeInGameRule(player, this));
            CheckRule(new GameMustBeNotStartedRule(this));
            return AcceptRequestWhenNotRequestedYet(player);
        }

        internal void SetRanking(Player player)
        {
            var nextRanking = _players.Count(x => x.CountCards() == 0);
            player.SetRanking(nextRanking);

        }

        internal void AddToDeck(IEnumerable<Card> cards, PlayerId playerId)
        {
            CheckRule(new CardsPlayedMustBeEqualsOrHigherRule(cards, _cards));
            _cards.Clear();
            _cards.AddRange(cards);
            SetLastPlayer(playerId);
            SetNextPlayer();
        }
        internal void SkipPlayer()
        {
            SetNextPlayer();
            if (HasOnePlayerRemaining())
            {
                StartNewTurn();
                _lastPlayer = null;
            }
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

        internal void CheckIsOver()
        {
            if (HasOnePlayerWithAnyCardRemaining())
            {
                SetLoser();
                GameOver();
                AddDomainEvent(new GameOver(_gameId));
            }
        }

        private Player GetViceAsshole()
        {
            return _players.First(x => x.IsAsshole(_players.Count - 1));
        }

        private void StartNewTurn()
        {
            foreach (var player in _players)
                player.HasSkip = false;
        }

        private void GameOver()
        {
            _currentPlayer = null;
            _hasStarted = false;
            _startRequests = new Dictionary<PlayerId, bool>(_startRequests.Select(x => new KeyValuePair<PlayerId, bool>(x.Key, false)));
        }

        private bool IsOrderingDone()
        {
            return _orders.Any();
        }

        private bool HasOnePlayerWithAnyCardRemaining()
        {
            return _players.Count(x => x.CountCards() > 0) == 1;
        }

        private void SetLoser()
        {
            _players.Single(x => x.CountCards() > 0).SetRanking(_players.Count);
        }

        private bool HasOnePlayerRemaining()
        {
            return _players.Where(x => !x.PlayerId.Equals(_currentPlayer)).All(x => x.HasSkip);
        }

        private void SetLastPlayer(PlayerId playerId)
        {
            _lastPlayer = playerId;
        }

        private void GiveCard(Card card, Player player)
        {
            player.Pickup(card);
        }

        private void SetNextPlayer()
        {
            var currentPlayer = _orders.FirstOrDefault(x => x.Value.Equals(_currentPlayer));
            _currentPlayer = currentPlayer.Key == _orders.Count - 1 ? _orders[0] : _orders[currentPlayer.Key + 1];
            if (GetPlayer(_currentPlayer.Value).HasSkip || GetPlayer(_currentPlayer.Value).CountCards() == 0)
            {
                SetNextPlayer();
                return;
            }
            AddDomainEvent(new CurrentPlayerChanged(_gameId, _currentPlayer.Value));
        }

        private Player GetPlayerWithLessCards()
        {
            var min = _players.Min(x => x.CountCards());
            return _players.FirstOrDefault(x => x.CountCards() == min);
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

        private bool HasAllPlayersRequestToStart()
        {
            return _startRequests.All(x => x.Value);
        }

        private bool IsFull()
        {
            return _players.Count == MAX_PLAYERS_AUTHORIZED;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Game);
        }

        private bool Equals(Game game)
        {
            return game._currentPlayer.Equals(_currentPlayer) &&
                    game._gameId.Equals(_gameId) &&
                    game._hasStarted == _hasStarted &&
                    game._lastPlayer.Equals(_lastPlayer) &&
                    game._cards.SequenceEqual(_cards) &&
                    game._players.SequenceEqual(_players) &&
                    game._startRequests.SequenceEqual(_startRequests) &&
                    game._orders.SequenceEqual(_orders);
        }
    }
}
