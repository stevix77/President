namespace President.Domain.Players.Rules
{
    using President.Domain.Cards;
    using President.Domain.Exceptions;
    using System.Collections.Generic;
    using System.Linq;

    internal class PlayerMustContainsCardsRule : IBusinessRule
    {
        private readonly IEnumerable<Card> _cardsPlayed;
        private readonly IEnumerable<Card> _cardsPlayer;

        public PlayerMustContainsCardsRule(IEnumerable<Card> cardsPlayed, List<Card> cardsPlayer)
        {
            _cardsPlayed = cardsPlayed;
            _cardsPlayer = cardsPlayer;
        }

        public void Check()
        {
            if (_cardsPlayed.Any(x => !_cardsPlayer.Contains(x)))
                throw new DomainException("Player must playing only cards that it has");
        }
    }
}
