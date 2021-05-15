using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Core.Domain.Entities
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _events;

        protected AggregateRoot() => _events = new List<IDomainEvent>();

        protected void Apply(IDomainEvent @event)
        {
            EnsureValidState();
            _events.Add(@event);
        }

        protected abstract void EnsureValidState();

        public IEnumerable<IDomainEvent> GetChanges() => _events.AsEnumerable();

        public void ClearChanges() => _events.Clear();

    }
}
