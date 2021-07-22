using System;
using System.Threading.Tasks;
using Lipar.Core.Domain.Events;
using System.Threading;
using Lipar.Core.Application.Common;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lipar.Core.Application.Events
{

    public class EventPublisher : IEventPublisher
    {
        private readonly ServiceFactory _serviceFactory;
        private static readonly ConcurrentDictionary<Type, EventHandlerWrapper> _eventHandlers = new();
        public EventPublisher(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task Raise<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent
        {
            var eventType = @event.GetType();
            var handler = _eventHandlers.GetOrAdd(eventType,
                static t => (EventHandlerWrapper)(Activator.CreateInstance(typeof(EventHandlerWrapperImpl<>).MakeGenericType(t))
                                                          ?? throw new InvalidOperationException($"Could not create wrapper for type {t}")));

            await handler.Handle(@event, cancellationToken, _serviceFactory, RaiseCore);
        }

        private async Task RaiseCore(IEnumerable<Func<IEvent, CancellationToken, Task>> allHandlers, IEvent @event, CancellationToken cancellationToken)
        {
            foreach (var handler in allHandlers)
                await handler(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}
