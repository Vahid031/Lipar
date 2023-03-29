using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events;

public class CategoryDeleted : DomainEvent
{
    public Guid Id { get; }

    public CategoryDeleted(Guid id) 
    {
        Id = id;
    }
}
