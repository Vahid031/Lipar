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
        public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _logger.LogInformation($"Publish event has succedded : {_json.SerializeObject(@event)}");

            return Task.CompletedTask;
        }

        public async Task Subscribe<TEvent>(string topic, CancellationToken cancellationToken) where TEvent : IEvent
        {
            await Subscribe(topic, typeof(TEvent), cancellationToken);
        }

        public Task Subscribe(string topic, Type type, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Subscribe event from {topic} has started");

            return Task.CompletedTask;
        }
    }
}