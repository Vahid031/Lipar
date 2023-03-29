using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Products.Events;

public class ProductDeleted : DomainEvent
{
    public Guid Id { get; }

    public ProductDeleted(Guid id) 
    {
        Id = id;
    }
}
