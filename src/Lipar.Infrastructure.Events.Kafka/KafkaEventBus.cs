using System;
using Confluent.Kafka;
using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using Lipar.Core.Contract.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Collections.Generic;

namespace KafkaEventBus;

public class KafkaEventBus : IEventBus
{
    private readonly IJsonService _jsonService;
    private readonly LiparOptions _liparOptions;

    public KafkaEventBus(IJsonService jsonService, LiparOptions liparOptions)
    {
        _jsonService = jsonService;
        _liparOptions = liparOptions;
    }

    public void Publish<TEvent>(TEvent input) where TEvent : IEvent
    {
        string messageName = input.GetType().Name;
        Parcel parcel = new Parcel
        {
            MessageId = Guid.NewGuid().ToString(),
            MessageBody = _jsonService.SerializeObject(input),
            MessageName = messageName,
            Route = $"{_liparOptions.ServiceId}.{messageName}",
            Headers = new Dictionary<string, object>
            {
                ["AccuredOn"] = DateTime.Now.ToString(),
            }
        };
        Send(parcel);
    }


    private void Send(Parcel parcel)
    {
        string topicName = "message_stream";

        var config = new ProducerConfig() { BootstrapServers = "172.31.38.195:31200" };

        using (var producer = new ProducerBuilder<string, Parcel>(config).Build())
        {

            var result = producer.ProduceAsync(topicName, new Message<string, Parcel> { Key = parcel.MessageId, Value = parcel }).GetAwaiter().GetResult();

            Console.WriteLine($"Event sent on Partition: {result.Partition} with Offset: {result.Offset}");

        }
    }

    public void Subscribe(string serviceId, string eventName)
    {
        string topicName = "message_stream";

        var config = new ConsumerConfig { GroupId = "messageConsumer", BootstrapServers = "172.31.38.195:31200", EnableAutoCommit = false };

        using (var consumer = new ConsumerBuilder<string, Parcel>(config).Build())
        {
            consumer.Subscribe(topicName);

            while (true)
            {
                var consumeResult = consumer.Consume();
                Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value.MessageName}");
                consumer.Commit();
            }
        }
    }

}

