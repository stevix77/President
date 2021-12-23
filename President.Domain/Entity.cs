using System.Collections.Generic;

namespace President.Domain
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get => _domainEvents.AsReadOnly(); }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected void CheckRule(IBusinessRule rule)
        {
            rule.Check();
        }
    }
}
