using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Products.Events;

public class ProductUpdated : IDomainEvent
{
    public string Id { get; }
    public string Name { get; }
    public string Barcode { get; }

    private ProductUpdated() { }

    public ProductUpdated(string id, string name, string barcode)
    {
        Id = id;
        Name = name;
        Barcode = barcode;
    }
}
