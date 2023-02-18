using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IEventBus
{
    Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    Task Subscribe(Dictionary<string, Type> topics, CancellationToken cancellationToken);
}


