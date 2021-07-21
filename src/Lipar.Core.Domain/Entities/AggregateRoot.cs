using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Core.Domain.Entities
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IEvent> _events;

        protected AggregateRoot() => _events = new List<IEvent>();

        public AggregateRoot(IEnumerable<IEvent> events)
        {
            if (events == null) return;
            foreach (var @event in events)
                ((dynamic)this).On((dynamic)@event);
        }

        protected void Apply(IEvent @event)
        {
            _events.Add(@event);
        }

        public IEnumerable<IEvent> GetChanges() => _events.AsEnumerable();

        public void ClearChanges() => _events.Clear();
    }
}
