using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Products.Events
{
    public class ProductUpdated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Barcode { get; }

        private ProductUpdated() { }

        public ProductUpdated(Guid id, string name, string barcode)
        {
            Id = id;
            Name = name;
            Barcode = barcode;
        }
    }
}
