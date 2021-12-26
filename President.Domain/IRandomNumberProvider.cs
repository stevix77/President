namespace President.Domain
{
    public interface IRandomNumberProvider
    {
        int GetNextNumber(int min, int max);
    }
}
