using Lipar.Core.Domain.Events;

namespace Market.Core.Domain.Products.Events
{
    public class ProductDeleted : IEvent
    {
        public string Id { get; }

        private ProductDeleted() { }

        public ProductDeleted(string id)
        {
            Id = id;
        }
    }
}
