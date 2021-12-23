namespace President.Application
{
    using System;
    using System.Runtime.Serialization;


    [Serializable]
    public class ApplicationException : Exception
    {

        public ApplicationException(string message) : base(message)
        {
        }
    }
}