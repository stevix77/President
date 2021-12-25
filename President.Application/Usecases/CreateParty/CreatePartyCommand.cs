namespace President.Application.Usecases.CreateParty
{
    public class CreatePartyCommand
    {
        public CreatePartyCommand(string gameId)
        {
            PartyId = gameId;
        }

        public string PartyId { get; }
    }
}
