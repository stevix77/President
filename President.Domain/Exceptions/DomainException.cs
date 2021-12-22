namespace President.Domain.Exceptions
{
    using System;


    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
