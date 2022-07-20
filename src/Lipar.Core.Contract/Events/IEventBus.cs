using Lipar.Core.Domain.Events;

namespace Lipar.Core.Contract.Events;

public interface IEventBus
{
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    void Subscribe(string serviceId, string eventName);
}


