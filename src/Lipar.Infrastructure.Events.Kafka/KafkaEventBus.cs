using System;
using Confluent.Kafka;
using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using Lipar.Core.Contract.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Reflection;
using System.Text;
using Lipar.Core.Contract.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Lipar.Infrastructure.Events.Kafka;

public class KafkaEventBus : IEventBus
{
    private readonly IJsonService _jsonService;
    private readonly LiparOptions _liparOptions;
    private readonly IInBoxEventRepository _inBoxEventRepository;
    private readonly IIntegrationEventDispatcher _eventPublisher;
    private readonly ILogger<KafkaEventBus> _logger;
    private readonly KafkaConsumerBuilder _kafkaConsumerBuilder;
    private readonly Lazy<IProducer<string, string>> _cachedProducer;

    public KafkaEventBus(IJsonService jsonService,
        LiparOptions liparOptions,
        IInBoxEventRepository inBoxEventRepository,
        IIntegrationEventDispatcher eventPublisher,
        ILogger<KafkaEventBus> logger)
    {
        _jsonService = jsonService;
        _liparOptions = liparOptions;
        _inBoxEventRepository = inBoxEventRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
        _kafkaConsumerBuilder = KafkaConsumerBuilder.CreateFactoty(liparOptions?.MessageBus?.Kafka);
        _cachedProducer = new Lazy<IProducer<string, string>>(() =>
                    KafkaProducerBuilder.CreateFaktory(liparOptions?.MessageBus?.Kafka).Build());
    }

    public async Task Publish<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent
    {
        string topic = @event.GetType().GetCustomAttribute<EventTopicAttribute>()?.Topic;

        if (string.IsNullOrEmpty(topic))
        {
            _logger.LogError($"Topic has not set for {@event}");
            throw new ArgumentNullException(nameof(EventTopicAttribute));
        }
        var producedMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = _jsonService.SerializeObject(@event),
            Timestamp = Timestamp.Default,
            Headers = new Headers()
            {
                new Header("TypeName",Encoding.UTF8.GetBytes( @event.GetType().Name)),
                new Header("ServiceId",Encoding.UTF8.GetBytes( _liparOptions.ServiceId))
            }
        };

        await _cachedProducer.Value.ProduceAsync(topic, producedMessage);
    }

    public async Task Subscribe(Dictionary<string, string> topics, CancellationToken cancellationToken)
    {
        if (!topics.Any())
        {
            _logger.LogWarning($"Dosn't exist any topic to subscribe");
            return;
        }
        using var consumer = _kafkaConsumerBuilder.Build();
        consumer.Subscribe(topics.Keys.ToList());

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);
                string serviceId;
                if (consumeResult.Message.Headers.TryGetLastBytes("ServiceId", out byte[] bytes))
                    serviceId = Encoding.UTF8.GetString(bytes);
                else
                {
                    _logger.LogWarning($"Event has not ServiceId - messageId : {consumeResult.Message.Key}");
                    continue;
                }

                if (consumeResult.Message?.Value is null)
                {
                    _logger.LogWarning($"Event has no body - messageId : {consumeResult.Message.Key}");
                    continue;
                }

                var @event = new InBoxEvent
                {
                    MessageId = consumeResult.Message.Key,
                    OwnerService = serviceId,
                    Paload = consumeResult.Message.Value,
                    ReceivedAt = DateTime.Now,
                    TypeName = topics[consumeResult.Topic],
                    RetryCount = 0,
                    Status = InBoxEventStatus.Scheduled
                };

                await _inBoxEventRepository.ReceiveNewEvent(@event);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "an error has been accured!");

                //var producedMessage = new Message<string, string>
                //{
                //    Key = Guid.NewGuid().ToString(),
                //    Value = null,
                //    Timestamp = Timestamp.Default,
                //    Headers = new Headers()
                //    {
                //        new Header("ServiceId",Encoding.UTF8.GetBytes( _liparOptions.ServiceId))
                //    }
                //};

                //await _cachedProducer.Value.ProduceAsync(ex.ConsumerRecord.Topic, producedMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error has been accured!");
            }
        }
        consumer.Close();

    }
}

