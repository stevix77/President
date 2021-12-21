namespace President.Domain.Games.Exceptions
{
    using System;

    [Serializable]
    internal class GameIdException : Exception
    {
        public GameIdException(string message) : base(message)
        {
        }
    }
}