namespace President.Application.Usecases.Skip
{
    using President.Domain.Games;
    using President.Domain.Players;
    using System.Threading.Tasks;


    public class SkipCommandHandler
    {
        private readonly IGameRepository _gameRepository;

        public SkipCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task Handle(SkipCommand command)
        {
            var game = await _gameRepository.GetByIdAsync(new GameId(command.GameId));
            game.Skip();
        }
    }
}
