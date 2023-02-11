using Lipar.Core.Domain.Events;
using System;

namespace Lipar.Core.Contract.Events;

public interface IEventBus
{
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    void Subscribe<TEvent>(string topic) where TEvent : IEvent;
    void Subscribe(string topic, Type type);
}


