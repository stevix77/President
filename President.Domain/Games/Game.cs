﻿namespace President.Domain.Games
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
        private PlayerId? _lastPlayer;
        private readonly Dictionary<int, PlayerId> _orders;
        private readonly List<Player> _players;
        private readonly Dictionary<PlayerId, bool> _startRequests;
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
            _hasBegan = hasBegan;
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

        public void OrderPlayers(IRandomNumberProvider randomNumberProvider)
        {
            for(var i = 0; i< _players.Count;i++)
            {
                var number = randomNumberProvider.GetNextNumber(1, _players.Count);
                var player = _players.ElementAt(number - 1);
                _orders.Add(i, player.PlayerId);
                player.SetOrder(i);
            }
            _currentPlayer = _orders[0];
        }

        public void Distribute(Card[] cards)
        {
            foreach(var card in cards)
            {
                GiveCard(card, GetPlayerWithLessCards());
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
                _hasBegan = true;
                AddDomainEvent(new GameStarted(_gameId));
            }
        }

        internal bool HasBegan() => _hasBegan;
        internal bool IsRequestFromPlayerAccepted(Player player)
        {
            CheckRule(new PlayerMustBeInGameRule(player, this));
            CheckRule(new GameMustBeNotStartedRule(this));
            return AcceptRequestWhenNotRequestedYet(player);
        }

        internal void AddToDeck(IEnumerable<Card> cards, PlayerId playerId)
        {
            CheckRule(new CardsPlayedMustBeEqualsOrHigherRule(cards, _cards));
            _cards.Clear();
            _cards.AddRange(cards);
            SetLastPlayer(playerId);
            SetNextPlayer();
        }

        private void SetLastPlayer(PlayerId playerId)
        {
            _lastPlayer = playerId;
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

        private void GiveCard(Card card, Player player)
        {
            player.GetCard(card);
        }

        private void SetNextPlayer()
        {
            var currentPlayer = _orders.FirstOrDefault(x => x.Value.Equals(_currentPlayer));
            _currentPlayer = currentPlayer.Key == _players.Count - 1 ? _orders[0] : _orders[currentPlayer.Key + 1];
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
            return _players.Count == 6;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Game);
        }

        private bool Equals(Game game)
        {
            return ToString() == game.ToString();
        }

        public override string ToString()
        {
            return $"{_gameId} - {_hasBegan} - {string.Join(",", _players)} - {string.Join(",", _startRequests)}" +
                $" - last cards: {string.Join(",", _cards)} - ordering: {string.Join(",", _orders)} - currentPlayer: {_currentPlayer} - lastPlayer: {_lastPlayer}";
        }
    }
}
