namespace President.Domain.Parties
{
    public struct PartyId
    {
        private readonly string _value;

        public PartyId(string partyId)
        {
            if (string.IsNullOrEmpty(partyId))
                throw new PartyIdException("Party id cannot be null");
            _value = partyId;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
