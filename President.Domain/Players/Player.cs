namespace President.Domain.Players
{
    using President.Domain.Games;


    public class Player
    {
        private readonly PlayerId _playerId;

        public Player(PlayerId playerId)
        {
            _playerId = playerId;
        }

        public PlayerId PlayerId { get => _playerId; }

        public void Join(Game game)
        {
            if(!game.HasBegan())
                game.AddPlayer(this);
        }

        public override bool Equals(object obj)
        {
            return this.ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return _playerId.ToString();
        }
    }
}
