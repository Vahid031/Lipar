using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IEventBus
{
    Task Publish<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent;
    Task Subscribe(Dictionary<string, Type> topics, CancellationToken cancellationToken);
}


