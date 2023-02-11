using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Market.Infrastructure.Events
{
    public class FakeEventBus : IEventBus
    {
        private readonly ILogger<FakeEventBus> _logger;
        private readonly IJsonService _json;

        public FakeEventBus(ILogger<FakeEventBus> logger, IJsonService json)
        {
            _logger = logger;
            _json = json;
        }
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _logger.LogInformation($"Publish event has succedded : {_json.SerializeObject(@event)}");
        }

        public void Subscribe<TEvent>(string topic) where TEvent : IEvent
        {
        }

        public void Subscribe(string topic, Type type)
        {
            _logger.LogInformation($"Subscribe event from {topic} has started");
        }
    }
}