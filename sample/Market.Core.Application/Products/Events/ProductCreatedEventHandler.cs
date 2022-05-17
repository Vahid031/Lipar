﻿using Lipar.Core.Contract.Events;
using Market.Core.Domain.Products.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Events
{
    public class ProductCreatedEventHandler : IEventHandler<ProductCreated>
    {
        public Task Handle(ProductCreated @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
