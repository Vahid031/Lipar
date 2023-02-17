using Lipar.Core.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IEventBus
{
    Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    Task Subscribe<TEvent>(string topic, CancellationToken cancellationToken) where TEvent : IEvent;
    Task Subscribe(string topic, Type type, CancellationToken cancellationToken);
}


