namespace President.Domain.Players
{
    public struct PlayerId
    {
        private readonly string _playerId;

        public PlayerId(string playerId)
        {
            _playerId = playerId;
        }
    }
}
