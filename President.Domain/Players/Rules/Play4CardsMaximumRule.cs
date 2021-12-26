namespace President.Domain.Players.Rules
{
    public class Play4CardsMaximumRule : IBusinessRule
    {
        private readonly int _countCards;

        public Play4CardsMaximumRule(int countCards)
        {
            this._countCards = countCards;
        }

        public void Check()
        {
            if (_countCards > 4)
                throw new System.Exception();
        }
    }
}
