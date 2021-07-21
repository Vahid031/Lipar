using Lipar.Core.Application.Events;
using Market.Core.Domain.Products.Events;
using System;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Events
{
    public class ProductCreatedEventHandler : IEventHandler<ProductCreated>
    {
        public Task Handle(ProductCreated @event)
        {
            throw new NotImplementedException();
        }
    }
}
