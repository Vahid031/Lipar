using System;
using Confluent.Kafka;
using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using Lipar.Core.Contract.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Collections.Generic;
using System.Reflection;
using Lipar.Core.Contract.Common;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using Lipar.Core.Contract.Data;

namespace Lipar.Infrastructure.Events.Kafka;

public class KafkaEventBus : IEventBus
{
    private readonly IJsonService _jsonService;
    private readonly LiparOptions _liparOptions;
    private readonly IInBoxEventRepository _inBoxEventRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly KafkaConsumerBuilder _kafkaConsumerBuilder;
    private readonly Lazy<IProducer<string, string>> _cachedProducer;

    public KafkaEventBus(IJsonService jsonService, LiparOptions liparOptions, IInBoxEventRepository inBoxEventRepository, IEventPublisher eventPublisher)
    {
        _jsonService = jsonService;
        _liparOptions = liparOptions;
        _inBoxEventRepository = inBoxEventRepository;
        _eventPublisher = eventPublisher;
        _kafkaConsumerBuilder = KafkaConsumerBuilder.CreateFactoty(liparOptions?.MessageBus?.Kafka);
        _cachedProducer = new Lazy<IProducer<string, string>>(() =>
                    KafkaProducerBuilder.CreateFaktory(liparOptions?.MessageBus?.Kafka).Build());
    }

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        string topic = @event.GetType().GetCustomAttribute<EventTopicAttribute>()?.Topic;

        if (string.IsNullOrEmpty(topic))
            throw new ArgumentNullException(nameof(EventTopicAttribute));

        Send(new Parcel
        {
            MessageId = Guid.NewGuid().ToString(),
            MessageBody = _jsonService.SerializeObject(@event),
            MessageName = @event.GetType().Name,
            Topic = topic,
            Headers = new Dictionary<string, object>
            {
                ["AccuredOn"] = DateTime.Now.ToString(),
            }
        });
    }


    private void Send(Parcel parcel)
    {
        var producedMessage = new Message<string, string>
        {
            Key = parcel.MessageId,
            Value = _jsonService.SerializeObject(parcel),
            Timestamp = Timestamp.Default
        };

        _cachedProducer.Value.Produce(parcel.Topic, producedMessage);
    }

    public void Subscribe<TEvent>(string topic) where TEvent : IEvent
    {
        Subscribe(topic, typeof(TEvent));
    }

    public void Subscribe(string topic, Type type)
    {

        using var consumer = _kafkaConsumerBuilder.Build();
        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            var parcel = JsonConvert.DeserializeObject<Parcel>(consumeResult.Message.Value);

            if (_inBoxEventRepository.AllowReceive(parcel.MessageId, parcel.ServiceId))
            {
                var @event = (IEvent)_jsonService.DeserializeObject(parcel.MessageBody, type); ;
                _eventPublisher.Raise(@event);
                _inBoxEventRepository.Receive(parcel.MessageId, parcel.ServiceId);
            }
            consumer.Commit();
        }

    }
}

