using President.Domain.Exceptions;

namespace President.Domain.Games.Rules
{
    internal class GameMustHaveMoreTwoPlayersRule : IBusinessRule
    {
        private readonly int _playersCount;

        public GameMustHaveMoreTwoPlayersRule(int playersCount)
        {
            this._playersCount = playersCount;
        }

        public void Check()
        {
            if (_playersCount < 3)
                throw new DomainException("Game must have more than 2 players to start");
        }
    }
}
