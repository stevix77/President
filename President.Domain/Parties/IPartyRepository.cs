namespace President.Domain.Parties
{
    using System.Threading.Tasks;


    public interface IPartyRepository
    {
        Task SaveAsync(Party party);
    }
}
