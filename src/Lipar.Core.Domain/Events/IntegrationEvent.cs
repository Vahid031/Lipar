using System;

namespace Lipar.Core.Domain.Events;

public abstract class IntegrationEvent
{
    public Guid Id { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public string Owner { get; private set; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.Now;
    }

    public IntegrationEvent(string owner) : base()
    {
        Owner = owner;
    }

    public IntegrationEvent(Guid id, DateTime createdOn, string owner) 
    {
        Id = id;
        CreatedOn = createdOn;
        Owner = owner;
    }
}

