using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;
internal class TestEventBus : IEventBus
{
    public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        return Task.CompletedTask;
    }

    public Task Subscribe(Dictionary<string, Type> topics, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
