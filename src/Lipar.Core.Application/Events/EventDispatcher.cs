using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Lipar.Core.Domain.Events;

namespace Lipar.Core.Application.Events
{

    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public EventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #region Event Dispatcher
        public async Task PublishDomainEventAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.Handle(@event);
            }
        }
        #endregion

    }
}
