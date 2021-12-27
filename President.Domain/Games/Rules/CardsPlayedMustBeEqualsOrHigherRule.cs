using President.Domain.Cards;
using President.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace President.Domain.Games.Rules
{
    public class CardsPlayedMustBeEqualsOrHigherRule : IBusinessRule
    {
        private readonly IEnumerable<Card> _cardsPlayed;
        private readonly IEnumerable<Card> _lastCards;

        public CardsPlayedMustBeEqualsOrHigherRule(IEnumerable<Card> cardsPlayed, IEnumerable<Card> lastCards)
        {
            _cardsPlayed = cardsPlayed;
            _lastCards = lastCards;
        }

        public void Check()
        {
            if (_cardsPlayed.Any(x => _lastCards.Any(y => y.Weight > x.Weight)))
                throw new DomainException("Cards must be equals or higher than last cards played");
        }
    }
}
