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

namespace Lipar.Infrastructure.Events.Kafka;

public class KafkaEventBus : IEventBus
{
    private readonly IJsonService _jsonService;
    private readonly LiparOptions _liparOptions;
    private readonly IInBoxEventRepository _inBoxEventRepository;
    private readonly IIntegrationEventPublisher _eventPublisher;
    private readonly KafkaConsumerBuilder _kafkaConsumerBuilder;
    private readonly Lazy<IProducer<string, string>> _cachedProducer;

    public KafkaEventBus(IJsonService jsonService, LiparOptions liparOptions, IInBoxEventRepository inBoxEventRepository, IIntegrationEventPublisher eventPublisher)
    {
        _jsonService = jsonService;
        _liparOptions = liparOptions;
        _inBoxEventRepository = inBoxEventRepository;
        _eventPublisher = eventPublisher;
        _kafkaConsumerBuilder = KafkaConsumerBuilder.CreateFactoty(liparOptions?.MessageBus?.Kafka);
        _cachedProducer = new Lazy<IProducer<string, string>>(() =>
                    KafkaProducerBuilder.CreateFaktory(liparOptions?.MessageBus?.Kafka).Build());
    }

    public async Task Publish<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent
    {
        string topic = @event.GetType().GetCustomAttribute<EventTopicAttribute>()?.Topic;

        if (string.IsNullOrEmpty(topic))
            throw new ArgumentNullException(nameof(EventTopicAttribute));

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

    public async Task Subscribe(Dictionary<string, Type> topics, CancellationToken cancellationToken)
    {
        if (!topics.Any())
            return;

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
                    throw new ArgumentNullException($"ServiceId is null on message : {consumeResult.Message.Key}");

                if (consumeResult.Message?.Value == null)
                    continue;

                if (_inBoxEventRepository.AllowReceive(consumeResult.Message.Key, serviceId))
                {

                    var @event = (IntegrationEvent)_jsonService.DeserializeObject(consumeResult.Message?.Value, topics[consumeResult.Topic]); ;
                    await _eventPublisher.Raise(@event);
                    await _inBoxEventRepository.Receive(consumeResult.Message.Key, serviceId);
                }

            }
            catch (ConsumeException ex)
            {
                var producedMessage = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = null,
                    Timestamp = Timestamp.Default,
                    Headers = new Headers()
                    {
                        new Header("ServiceId",Encoding.UTF8.GetBytes( _liparOptions.ServiceId))
                    }
                };

                await _cachedProducer.Value.ProduceAsync(ex.ConsumerRecord.Topic, producedMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
        consumer.Close();

    }
}

