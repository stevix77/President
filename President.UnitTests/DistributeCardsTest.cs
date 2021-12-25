namespace President.UnitTests
{
    using President.Application.Usecases.DistributeCards;
    using President.Domain.Games;
    using President.Domain.Players;
    using President.Infrastructure.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;


    public class DistributeCardsTest
    {
        [InlineData(3)]
        [InlineData(5)]
        [Theory]
        public async Task GameStartedShouldDistributeCardsToPlayers(int countPlayers)
        {
            var players = GeneratePlayers(countPlayers);
            var game = Game.FromState(new("g1", true, players.ToArray(), Array.Empty<PlayerId>()));
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
