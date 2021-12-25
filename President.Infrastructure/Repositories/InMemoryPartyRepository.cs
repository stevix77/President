using President.Domain.Parties;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace President.Infrastructure.Repositories
{
    public class InMemoryPartyRepository : IPartyRepository
    {
        private readonly List<Party> _parties;
        public InMemoryPartyRepository(Party party = null)
        {
            _parties = new List<Party>();
            if (party != null)
                _parties.Add(party);
        }
        public Task SaveAsync(Party party)
        {
            if (_parties.Any(x => x.PartyId.Equals(party.PartyId)))
                return Task.CompletedTask;
            _parties.Add(party);
            return Task.CompletedTask;
        }

        public Party GetParty(string gameId)
        {
            return _parties.Any() ? _parties[0] : null;
        }

        public int CountParties() => _parties.Count;
    }
}
