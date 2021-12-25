namespace President.Application.Usecases.CreateParty
{
    using President.Domain.Parties;
    using System.Threading.Tasks;


    public class CreatePartyCommandHandler
    {
        private readonly IPartyRepository _partyRepository;
        public CreatePartyCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task Handle(CreatePartyCommand command)
        {
            var party = new Party(new PartyId(command.PartyId));
            await _partyRepository.SaveAsync(party).ConfigureAwait(false);
        }
    }
}
