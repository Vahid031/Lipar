﻿using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Products.Events;

namespace Market.Core.Domain.Products.Entities
{
    public class Product : AggregateRoot, IAuditable
    {
        public string Name { get; private set; }
        public string Barcode { get; private set; }

        private Product() { }

        public Product(EntityId id, string name, string barcode)
        {
            Id = id;
            Name = name;
            Barcode = barcode;

            Apply(new ProductCreated(Id.ToString(), Barcode, Name));
        }

        public void Update(string name, string barcode)
        {
            Name = name;
            Barcode = barcode;

            Apply(new ProductUpdated(Id.ToString(), Barcode, Name));
        }

        public void Delete(EntityId id)
        {
            Apply(new ProductDeleted(Id.ToString()));
        }
    }
}
