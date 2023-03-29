using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Common;
using Lipar.Core.Domain.Events;

namespace Lipar.Core.Application.Events;


public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ServiceFactory _serviceFactory;
    private static readonly ConcurrentDictionary<Type, DomainEventHandlerWrapper> _eventHandlers = new();
    public DomainEventDispatcher(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async Task Raise<TDomainEvent>(TDomainEvent @event, CancellationToken cancellationToken = default) where TDomainEvent : DomainEvent
    {
        var eventType = @event.GetType();
        var handler = _eventHandlers.GetOrAdd(eventType,
        static t => (DomainEventHandlerWrapper)(Activator.CreateInstance(typeof(DomainEventHandlerWrapperImpl<>).MakeGenericType(t))
    ?? throw new InvalidOperationException($"Could not create wrapper for type {t}")));

        await handler.Handle(@event, cancellationToken, _serviceFactory, RaiseCore);
    }

    private async Task RaiseCore(IEnumerable<Func<DomainEvent, CancellationToken, Task>> allHandlers, DomainEvent @event, CancellationToken cancellationToken)
    {
        foreach (var handler in allHandlers)
            await handler(@event, cancellationToken).ConfigureAwait(false);
    }
}


