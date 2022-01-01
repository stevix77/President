namespace President.Application.Usecases.GiveCards
{
    using President.Domain.Cards;
    using System.Collections.Generic;

    public class GiveCardsCommand
    {
        public GiveCardsCommand(string gameId, Card[] cards)
        {
            GameId = gameId;
            Cards = cards;
        }

        public string GameId { get; }
        public IEnumerable<Card> Cards { get; }
    }
}
