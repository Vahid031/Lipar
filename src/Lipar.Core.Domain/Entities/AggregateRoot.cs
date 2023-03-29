using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Core.Domain.Entities;

public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _events;

    protected AggregateRoot() => _events = new List<DomainEvent>();

    public AggregateRoot(IEnumerable<DomainEvent> events)
    {
        if (events == null) return;
        foreach (var @event in events)
            ((dynamic)this).On((dynamic)@event);
    }

    protected void Apply(DomainEvent @event)
    {
        @event.SetAggregateId(Id.Value);
        _events.Add(@event);
    }

    public IEnumerable<DomainEvent> GetEvents() => _events.AsEnumerable();

    public void ClearEvents() => _events.Clear();
}


