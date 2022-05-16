using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Extensions;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Events
{
    public abstract class EventHandlerWrapper
    {
        public abstract Task Handle(IEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
                                    Func<IEnumerable<Func<IEvent, CancellationToken, Task>>, IEvent, CancellationToken, Task> publish);
    }

    public class EventHandlerWrapperImpl<TEvent> : EventHandlerWrapper
       where TEvent : IEvent
    {
        public override Task Handle(IEvent Event, CancellationToken cancellationToken, ServiceFactory serviceFactory,
                                    Func<IEnumerable<Func<IEvent, CancellationToken, Task>>, IEvent, CancellationToken, Task> publish)
        {
            var handlers = serviceFactory
                .GetInstances<IEventHandler<TEvent>>()
                .Select(x => new Func<IEvent, CancellationToken, Task>((theEvent, theToken) => x.Handle((TEvent)theEvent, theToken)));

            return publish(handlers, Event, cancellationToken);
        }
    }
}
