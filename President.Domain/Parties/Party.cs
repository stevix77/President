namespace President.Domain.Parties
{
    public class Party
    {
        private PartyId _partyId;

        public Party(PartyId partyId)
        {
            _partyId = partyId;
        }

        public PartyId PartyId { get => _partyId; }

        public override bool Equals(object obj)
        {
            return _partyId.Equals((obj as Party)._partyId);
        }

        public override string ToString()
        {
            return _partyId.ToString();
        }
    }
}
