using President.Domain.Exceptions;
using President.Domain.Games;

namespace President.Domain.Players.Rules
{
    class SkipFromThisTurnRule : IBusinessRule
    {
        private readonly PlayerId _playerId;
        private readonly Game _game;

        public SkipFromThisTurnRule(PlayerId playerId, Game game)
        {
            _playerId = playerId;
            _game = game;
        }

        public void Check()
        {
            if (!_playerId.Equals(_game.CurrentPlayer))
                throw new DomainException($"Player {_playerId} must waiting to skip");
        }
    }
}
