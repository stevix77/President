namespace President.Domain.Players
{
    using President.Domain.Cards;
    using President.Domain.Games;
    using President.Domain.Players.Events;
    using President.Domain.Players.Rules;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Player : Entity
    {
        private readonly PlayerId _playerId;
        private readonly List<Card> _cards;
        private int _order;
        private bool _hasSkip;
        private int _rank;

        private Player(PlayerId playerId, int order, Card[] cards, bool hasSkip, int rank)
        {
            _playerId = playerId;
            _cards = cards.ToList();
            _order = order;
            _hasSkip = hasSkip;
            _rank = rank;
        }

        public void GiveCards(IEnumerable<Card> cards, Player player)
        {
            var cardsToGive = _cards.Where(x => cards.Contains(x));
            player.AddCards(cardsToGive);
            _cards.RemoveAll(x => cards.Contains(x));
        }

        public void GiveBestCards(int countCardsToGive, Player player)
        {
            var cards = _cards.OrderByDescending(x => x.Weight).Take(countCardsToGive);
            player.AddCards(cards);
            _cards.RemoveAll(x => cards.Contains(x));
        }

        public static Player FromState(PlayerState state)
        {
            return new Player(state.PlayerId,
                              state.Order,
                              state.Cards,
                              state.HasSkip,
                              state.Rank);
        }

        public void Skip(Game game)
        {
            CheckRule(new SkipFromThisTurnRule(_playerId, game));
            _hasSkip = true;
            game.SkipPlayer();
        }

        public PlayerId PlayerId { get => _playerId; }
        public bool HasSkip { get => _hasSkip; internal set => _hasSkip = value; }
        public int Rank { get => _rank; }

        public int CountCards()
        {
            return _cards.Count;
        }

        public void RequestStartingGame(Game game)
        {
            if (game.IsRequestFromPlayerAccepted(this))
                AddDomainEvent(new StartRequested(_playerId, game.GameId));
        }

        public void Play(IEnumerable<Card> cards, Game game)
        {
            CheckRule(new Play4CardsMaximumRule(cards.Count()));
            CheckRule(new PlayerMustWaitingToPlayRule(_playerId, game));
            CheckRule(new PlayerMustContainsCardsRule(cards, _cards));
            game.AddToDeck(cards, _playerId);
            DropCards(cards);
            if (HasNoCard())
            {
                game.SetRanking(this);
                game.CheckIsOver();
            }
        }

        public void Join(Game game)
        {
            game.AddPlayer(this);
            AddDomainEvent(new GameJoined(_playerId, game.GameId));
        }

        internal void AddCards(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        internal void Pickup(Card card)
        {
            _cards.Add(card);
        }

        internal void SetOrder(int i)
        {
            _order = i;
        }

        internal void SetRanking(int rank)
        {
            _rank = rank;
            if (IsWinner())
                AddDomainEvent(new GameWon(_playerId));
        }

        internal bool IsAsshole(int count)
        {
            return _rank == count;
        }

        private bool IsWinner() => _rank == 1;

        bool HasNoCard()
        {
            return CountCards() == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        private bool Equals(Player player)
        {
            return _hasSkip == player._hasSkip &&
                    _order == player._order &&
                    _playerId.Equals(player._playerId) &&
                    _cards.SequenceEqual(player._cards) &&
                    _rank == player._rank;
        }

        private void DropCards(IEnumerable<Card> cards)
        {
            _cards.RemoveAll(x => cards.Any(y => y.Equals(x)));
        }
    }
}
