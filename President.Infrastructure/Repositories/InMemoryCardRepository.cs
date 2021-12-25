namespace President.Infrastructure.Repositories
{
    using President.Domain.Cards;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using static President.Domain.Cards.Card;
    using System;

    public class InMemoryCardRepository : ICardRepository
    {
        private readonly List<Card> _cards;
        public InMemoryCardRepository()
        {
            _cards = new List<Card>();
            InitCards();
        }

        public Task<Card[]> GetCards()
        {
            return Task.FromResult(_cards.OrderBy(x => new Random().Next()).ToArray());
        }

        private void InitCards()
        {
            _cards.AddRange(InitClubs());
            _cards.AddRange(InitHearts());
            _cards.AddRange(InitDiamonds());
            _cards.AddRange(InitSpades());
        }

        private static Card CreateSpade(int weight, string name)
        {
            return new Card(weight, name, Color.SPADE);
        }

        private static Card CreateDiamond(int weight, string name)
        {
            return new Card(weight, name, Color.DIAMOND);
        }

        private static Card CreateHeart(int weight, string name)
        {
            return new Card(weight, name, Color.HEART);
        }

        private static Card CreateClub(int weight, string name)
        {
            return new Card(weight, name, Color.CLUB);
        }

        private static IEnumerable<Card> InitSpades()
        {
            return new Card[]
            {
                CreateSpade(1, "3"),
                CreateSpade(2, "4"),
                CreateSpade(3, "5"),
                CreateSpade(4, "6"),
                CreateSpade(5, "7"),
                CreateSpade(6, "8"),
                CreateSpade(7, "9"),
                CreateSpade(8, "10"),
                CreateSpade(9, "J"),
                CreateSpade(10, "Q"),
                CreateSpade(11, "K"),
                CreateSpade(12, "A"),
                CreateSpade(13, "2")
            };
        }

        private static IEnumerable<Card> InitDiamonds()
        {
            return new Card[]
            {
                CreateDiamond(1, "3"),
                CreateDiamond(2, "4"),
                CreateDiamond(3, "5"),
                CreateDiamond(4, "6"),
                CreateDiamond(5, "7"),
                CreateDiamond(6, "8"),
                CreateDiamond(7, "9"),
                CreateDiamond(8, "10"),
                CreateDiamond(9, "J"),
                CreateDiamond(10, "Q"),
                CreateDiamond(11, "K"),
                CreateDiamond(12, "A"),
                CreateDiamond(13, "2")
            };
        }

        private static IEnumerable<Card> InitHearts()
        {
            return new Card[]
            {
                CreateHeart(1, "3"),
                CreateHeart(2, "4"),
                CreateHeart(3, "5"),
                CreateHeart(4, "6"),
                CreateHeart(5, "7"),
                CreateHeart(6, "8"),
                CreateHeart(7, "9"),
                CreateHeart(8, "10"),
                CreateHeart(9, "J"),
                CreateHeart(10, "Q"),
                CreateHeart(11, "K"),
                CreateHeart(12, "A"),
                CreateHeart(13, "2")
            };
        }

        private static IEnumerable<Card> InitClubs()
        {
            return new Card[]
            {
                CreateClub(1, "3"),
                CreateClub(2, "4"),
                CreateClub(3, "5"),
                CreateClub(4, "6"),
                CreateClub(5, "7"),
                CreateClub(6, "8"),
                CreateClub(7, "9"),
                CreateClub(8, "10"),
                CreateClub(9, "J"),
                CreateClub(10, "Q"),
                CreateClub(11, "K"),
                CreateClub(12, "A"),
                CreateClub(13, "2")
            };
        }
    }
}
