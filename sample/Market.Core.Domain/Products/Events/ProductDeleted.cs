using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Products.Events
{
    public class ProductDeleted : IEvent
    {
        public Guid Id { get; }

        private ProductDeleted() { }

        public ProductDeleted(Guid id)
        {
            Id = id;
        }
    }
}
