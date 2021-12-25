namespace President.Application.Usecases.CreateGame
{
    using President.Domain.Games;
    using President.Domain.Games.Exceptions;
    using System.Threading.Tasks;


    public class CreateGameCommandHandler
    {
        private readonly IGameRepository _gameRepository;
        public CreateGameCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(CreateGameCommand command)
        {
            if(await _gameRepository.GetByIdAsync(new GameId(command.GameId)) != null)
                throw new GameAlreadyExistsException("A game with same id already exists");

            var game = new Game(new GameId(command.GameId));
            await _gameRepository.SaveAsync(game).ConfigureAwait(false);
        }
    }
}
