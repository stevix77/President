namespace President.Domain.Players.Rules
{
    using President.Domain.Exceptions;
    using President.Domain.Games;


    internal class PlayerMustWaitingToPlayRule : IBusinessRule
    {
        private readonly PlayerId _playerId;
        private readonly Game _game;

        internal PlayerMustWaitingToPlayRule(PlayerId playerId, Game game)
        {
            _playerId = playerId;
            _game = game;
        }

        public void Check()
        {
            if (!_playerId.Equals(_game.CurrentPlayer))
                throw new DomainException($"Player {_playerId} must waiting his turn to play");
        }
    }
}
