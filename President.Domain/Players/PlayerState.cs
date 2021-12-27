namespace President.Domain.Players
{
    using President.Domain.Cards;


    public class PlayerState
    {
        private readonly PlayerId _playerId;
        private readonly int _order;
        private readonly Card[] _cards;

        public PlayerState(PlayerId playerId, int order, Card[] cards)
        {
            _playerId = playerId;
            _order = order;
            _cards = cards;
        }

        public PlayerId PlayerId { get => _playerId; }
        public int Order { get => _order; }
        public Card[] Cards { get => _cards; }
    }
}
