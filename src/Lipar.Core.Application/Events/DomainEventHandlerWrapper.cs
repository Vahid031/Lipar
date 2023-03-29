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

public abstract class DomainEventHandlerWrapper
{
    public abstract Task Handle(DomainEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
    Func<IEnumerable<Func<DomainEvent, CancellationToken, Task>>, DomainEvent, CancellationToken, Task> publish);
}

public class DomainEventHandlerWrapperImpl<TDomainEvent> : DomainEventHandlerWrapper
where TDomainEvent : DomainEvent
{
    public override Task Handle(DomainEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
    Func<IEnumerable<Func<DomainEvent, CancellationToken, Task>>, DomainEvent, CancellationToken, Task> publish)
    {
        var handlers = serviceFactory
        .GetInstances<IDomainEventHandler<TDomainEvent>>()
        .Select(x => new Func<DomainEvent, CancellationToken, Task>((theEvent, theToken) => x.Handle((TDomainEvent)theEvent, theToken)));

        return publish(handlers, Event, cancellationToken);
    }
}

