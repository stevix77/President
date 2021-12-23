using President.Domain.Exceptions;
using President.Domain.Games;

namespace President.Domain
{
    public class GameMustBeNotStartedRule : IBusinessRule
    {
        private readonly Game _game;

        public GameMustBeNotStartedRule(Game game)
        {
            _game = game;
        }

        public void Check()
        {
            if (_game.HasBegan())
                throw new DomainException("The game must be not started");
        }
    }
}