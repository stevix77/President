using President.Application.Usecases.CreateParty;
using President.Domain.Parties;
using President.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace President.UnitTests
{
    public class PartyCreationHandlerTest
    {
        [Fact]
        public async Task ShouldCreateParty()
        {
            await AssertThatPartyCreated();
        }

        [Fact]
        public async Task ShouldNotCreatePartyWhenIdNull()
        {
            InMemoryPartyRepository partyRepository = new InMemoryPartyRepository();
            var command = new CreatePartyCommand(null);
            var handler = new CreatePartyCommandHandler(partyRepository);

            var record = await Record.ExceptionAsync(() => handler.Handle(command));

            Assert.Null(partyRepository.GetParty(command.PartyId));
        }

        [Fact]
        public async Task ShouldNotCreatePartyWhenIdAlreadyExist()
        {
            var party = new Party(new PartyId("party1"));
            InMemoryPartyRepository partyRepository = new InMemoryPartyRepository(party);
            var command = new CreatePartyCommand("party1");
            var handler = new CreatePartyCommandHandler(partyRepository);

            var record = await Record.ExceptionAsync(() => handler.Handle(command));

            Assert.Equal(1, partyRepository.CountParties());
        }

        private static async Task AssertThatPartyCreated()
        {
            InMemoryPartyRepository partyRepository = new InMemoryPartyRepository();
            var command = new CreatePartyCommand("game1");
            var handler = new CreatePartyCommandHandler(partyRepository);

            await handler.Handle(command);

            Assert.NotNull(partyRepository.GetParty("game1"));
        }
    }
}
