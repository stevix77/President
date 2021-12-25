namespace President.Application.Usecases.DistributeCards
{
    public class DistributeCardsCommand
    {
        private readonly string v;

        public DistributeCardsCommand(string v)
        {
            this.v = v;
        }
    }
}
