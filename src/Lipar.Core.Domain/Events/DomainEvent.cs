using System;

namespace Lipar.Core.Domain.Events;

public abstract class DomainEvent
{
    public Guid AggregateId { get; private set; }
    public DomainEventType DomainEventType { get; }

    public DomainEvent() => DomainEventType = DomainEventType.BeforeCommit;

    public DomainEvent(Guid aggregateId) : base() => AggregateId = aggregateId;

    public DomainEvent(Guid aggregateId, DomainEventType domainEventType)
    {
        AggregateId = aggregateId;
        DomainEventType = domainEventType;
    }

    public void SetAggregateId(Guid aggregateId) => AggregateId = aggregateId;
}

public enum DomainEventType
{
    BeforeCommit,
    AfterCommit
}

