namespace President.UnitTests
{
    using President.Application.Usecases.DistributeCards;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using President.UnitTests.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;


    public class DistributeCardsTest
    {
        private readonly GameStateBuilder _gameStateBuilder;
        public DistributeCardsTest()
        {
            _gameStateBuilder = new GameStateBuilder();
        }

        [InlineData(3)]
        [InlineData(5)]
        [Theory]
        public async Task GameStartedShouldDistributeCardsToPlayers(int countPlayers)
        {
            var players = GeneratePlayers(countPlayers);
            var game = Game.FromState(_gameStateBuilder.WithHasBegan(true)
                                                       .WithPlayers(players)
                                                       .Build());
            await DistributeCards(game, new DistributeCardsCommand("g1")).ConfigureAwait(false);
            AssertThatPlayersHaveEquitableCountCards(players);
        }

        private static void AssertThatPlayersHaveEquitableCountCards(IEnumerable<Player> players)
        {
            var countPlayers = players.Count();
            Assert.All(players, x =>
            {
                var index = players.ToList().IndexOf(x);
                Assert.Equal(52 / countPlayers + (index < (52 % countPlayers) ? 1 : 0), x.CountCards());
            });
        }

        private IEnumerable<Player> GeneratePlayers(int countPlayers)
        {
            var players = new List<Player>();
            for (var i = 0; i < countPlayers; i++)
                players.Add(new Player(new($"p{i}")));
            return players;
        }

        private static async Task DistributeCards(Game game, DistributeCardsCommand command)
        {
            var gameRepository = new InMemoryGameRepository(game);
            var handler = new DistributeCardsCommandHandler(gameRepository, new InMemoryCardRepository());
            await handler.Handle(command).ConfigureAwait(false);
        }
    }
}
