namespace President.Domain.Games.Exceptions
{
    using System;


    public class GameAlreadyExistsException : Exception
    {
        public GameAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
