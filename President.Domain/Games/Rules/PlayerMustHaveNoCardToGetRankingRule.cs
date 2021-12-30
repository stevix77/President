namespace President.Domain.Games.Rules
{
    using President.Domain.Exceptions;


    internal class PlayerMustHaveNoCardToGetRankingRule : IBusinessRule
    {
        private readonly int _countCards;

        public PlayerMustHaveNoCardToGetRankingRule(int countCards)
        {
            this._countCards = countCards;
        }

        public void Check()
        {
            if (_countCards != 0)
                throw new DomainException("Player must have no card to get a ranking");
        }
    }
}
