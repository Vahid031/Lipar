using Lipar.Core.Domain.Events;

namespace Market.Core.Domain.Products.Events;

[EventTopic("MarketService.ProductCreated")]
public class ProductCreated : IDomainEvent
{
    public string Id { get; }
    public string Name { get; }
    public string Barcode { get; }

    private ProductCreated() { }

    public ProductCreated(string id, string name, string barcode)
    {
        Id = id;
        Name = name;
        Barcode = barcode;
    }
}
