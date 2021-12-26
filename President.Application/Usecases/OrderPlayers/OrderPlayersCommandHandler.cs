namespace President.Application.Usecases.OrderPlayers
{
    using President.Domain;
    using President.Domain.Games;
    using System.Threading.Tasks;


    public class OrderPlayersCommandHandler
    {
        private readonly IGameRepository _gameRepository;
        private readonly IRandomNumberProvider randomNumberProvider;

        public OrderPlayersCommandHandler(IGameRepository gameRepository, IRandomNumberProvider randomNumberProvider)
        {
            _gameRepository = gameRepository;
            this.randomNumberProvider = randomNumberProvider;
        }

        public async Task Handle(OrderPlayersCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId)).ConfigureAwait(false);
            game.OrderPlayers(randomNumberProvider);
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
