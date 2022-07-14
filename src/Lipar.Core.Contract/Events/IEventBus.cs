namespace Lipar.Core.Contract.Events;

public interface IEventBus
{
    void Publish<T>(T input);
    void Subscribe(string serviceId, string eventName);
}


