using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Extensions;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Events;

public abstract class IntegrationEventHandlerWrapper
{
    public abstract Task Handle(IntegrationEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
    Func<IEnumerable<Func<IntegrationEvent, CancellationToken, Task>>, IntegrationEvent, CancellationToken, Task> publish);
}


public class IntegrationEventHandlerWrapperImpl<TIntegrationEvent> : IntegrationEventHandlerWrapper
where TIntegrationEvent : IntegrationEvent
{
    public override Task Handle(IntegrationEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
    Func<IEnumerable<Func<IntegrationEvent, CancellationToken, Task>>, IntegrationEvent, CancellationToken, Task> publish)
    {
        var handlers = serviceFactory
        .GetInstances<IIntegrationEventHandler<TIntegrationEvent>>()
        .Select(x => new Func<IntegrationEvent, CancellationToken, Task>((theEvent, theToken) => x.Handle((TIntegrationEvent)theEvent, theToken)));

        return publish(handlers, Event, cancellationToken);
    }
}



