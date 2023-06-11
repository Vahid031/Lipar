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
        public Task Publish<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent
        {
            _logger.LogInformation($"Publish event has succedded : {_json.SerializeObject(@event)}");

            return Task.CompletedTask;
        }

        public Task Subscribe(Dictionary<string, string> topics, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Subscribe event from {topics} has started");

            return Task.CompletedTask;
        }
    }
}