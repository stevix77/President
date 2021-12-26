namespace President.Infrastructure
{
    using President.Domain;
    using System.Linq;


    public class InMemoryRandomProvider : IRandomNumberProvider
    {
        private readonly int[] randomNumbers;
        private int _callNumber = 0;

        public InMemoryRandomProvider(params int[] randomNumbers)
        {
            this.randomNumbers = randomNumbers;
        }

        public int GetNextNumber(int min, int max)
        {
            _callNumber++;
            return randomNumbers.ElementAt(_callNumber - 1);
        }
    }
}
