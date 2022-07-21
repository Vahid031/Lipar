using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;
internal class TestEventBus : IEventBus
{
    public void Publish<TEvent>(TEvent input) where TEvent : IEvent
    {

    }

    public void Subscribe(string serviceId, string eventName)
    {

    }
}
