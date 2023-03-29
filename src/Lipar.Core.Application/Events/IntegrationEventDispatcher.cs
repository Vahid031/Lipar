using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Common;
using Lipar.Core.Domain.Events;

namespace Lipar.Core.Application.Events;

public class IntegrationEventDispatcher : IIntegrationEventDispatcher
{
    private readonly ServiceFactory _serviceFactory;
    private static readonly ConcurrentDictionary<Type, IntegrationEventHandlerWrapper> _eventHandlers = new();
    public IntegrationEventDispatcher(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async Task Raise<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default) where TIntegrationEvent : IntegrationEvent
    {
        var eventType = @event.GetType();
        var handler = _eventHandlers.GetOrAdd(eventType,
        static t => (IntegrationEventHandlerWrapper)(Activator.CreateInstance(typeof(IntegrationEventHandlerWrapperImpl<>).MakeGenericType(t))
    ?? throw new InvalidOperationException($"Could not create wrapper for type {t}")));

        await handler.Handle(@event, cancellationToken, _serviceFactory, RaiseCore);
    }

    private async Task RaiseCore(IEnumerable<Func<IntegrationEvent, CancellationToken, Task>> allHandlers, IntegrationEvent @event, CancellationToken cancellationToken)
    {
        foreach (var handler in allHandlers)
            await handler(@event, cancellationToken).ConfigureAwait(false);
    }
}


