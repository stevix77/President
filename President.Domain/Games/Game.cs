using President.Domain.Players;
using System;
using System.Collections;
using System.Collections.Generic;

namespace President.Domain.Games
{
    public class Game
    {
        private GameId _gameId;

        public Game(GameId gameId)
        {
            _gameId = gameId;
            Players = new List<Player>();
        }

        public GameId GameId { get => _gameId; }
        public List<Player> Players { get; set; }

        internal void AddPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}
