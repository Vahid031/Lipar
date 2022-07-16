using Lipar.Core.Contract.Events;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;
internal class TestEventBus : IEventBus
{
    public void Publish<T>(T input)
    {

    }

    public void Subscribe(string serviceId, string eventName)
    {

    }
}
