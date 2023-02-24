using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;
internal class TestEventBus : IEventBus
{
    public Task Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IDomainEvent
    {
        return Task.CompletedTask;
    }

    public Task Subscribe(Dictionary<string, Type> topics, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
