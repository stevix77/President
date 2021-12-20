namespace President.Domain.Parties
{
    using System;

    [Serializable]
    internal class PartyIdException : Exception
    {
        public PartyIdException(string message) : base(message)
        {
        }
    }
}