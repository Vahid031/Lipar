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

public abstract class EventHandlerWrapper
{
    public abstract Task Handle(IDomainEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
    Func<IEnumerable<Func<IDomainEvent, CancellationToken, Task>>, IDomainEvent, CancellationToken, Task> publish);
}

public class EventHandlerWrapperImpl<TDomainEvent> : EventHandlerWrapper
where TDomainEvent : IDomainEvent
{
    public override Task Handle(IDomainEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
    Func<IEnumerable<Func<IDomainEvent, CancellationToken, Task>>, IDomainEvent, CancellationToken, Task> publish)
    {
        var handlers = serviceFactory
        .GetInstances<IDomainEventHandler<TDomainEvent>>()
        .Select(x => new Func<IDomainEvent, CancellationToken, Task>((theEvent, theToken) => x.Handle((TDomainEvent)theEvent, theToken)));

        return publish(handlers, Event, cancellationToken);
    }
}


