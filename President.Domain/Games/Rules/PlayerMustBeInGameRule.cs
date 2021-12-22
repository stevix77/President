using President.Domain.Exceptions;
using President.Domain.Players;
using System;

namespace President.Domain.Games.Rules
{
    public class PlayerMustBeInGameRule : IBusinessRule
    {
        private readonly Player _player;
        private readonly Game _game;
        private const string MESSAGE = "Player is not in this game";
        public PlayerMustBeInGameRule(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        public void Check()
        {
            if (!_game.ContainsPlayer(_player))
                throw new DomainException(MESSAGE);
        }
    }
}
