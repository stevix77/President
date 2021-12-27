namespace President.Application.Usecases.PlayCards
{
    using President.Domain.Cards;
    using System.Collections.Generic;


    public class PlayCardsCommand
    {
        public PlayCardsCommand(string gameId, string playerId, IEnumerable<Card> cards)
        {
            GameId = gameId;
            PlayerId = playerId;
            Cards = cards;
        }

        public string GameId { get; }
        public string PlayerId { get; }
        public IEnumerable<Card> Cards { get; }
    }
}
