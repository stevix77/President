namespace President.Domain.Players
{
    using President.Domain.Cards;


    public class PlayerState
    {
        private readonly PlayerId _playerId;
        private readonly int _order;
        private readonly Card[] _cards;
        private readonly bool _hasSkip;
        private readonly int _rank;

        public PlayerState(PlayerId playerId, int order, Card[] cards, bool hasSkip, int rank)
        {
            _playerId = playerId;
            _order = order;
            _cards = cards;
            _hasSkip = hasSkip;
            _rank = rank;
        }

        public PlayerId PlayerId { get => _playerId; }
        public int Order { get => _order; }
        public Card[] Cards { get => _cards; }
        public bool HasSkip { get => _hasSkip; }
        public int Rank { get => _rank; }
    }
}
