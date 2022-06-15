using Lipar.Core.Domain.Events;

namespace Market.Core.Domain.Products.Events
{
    public class ProductDeleted : IEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Barcode { get; }

        private ProductDeleted() { }

        public ProductDeleted(string id)
        {
            Id = id;
        }
    }
}
