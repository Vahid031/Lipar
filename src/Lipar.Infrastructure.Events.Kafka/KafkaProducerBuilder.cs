using Confluent.Kafka;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System;

namespace Lipar.Infrastructure.Events.Kafka;

public class KafkaProducerBuilder
{
    private readonly KafkaOptions _kafkaOptions;
    private static KafkaProducerBuilder _instance;

    public static KafkaProducerBuilder CreateFaktory(KafkaOptions kafkaOptions)
    {
        if (_instance == null)
            _instance = new KafkaProducerBuilder(kafkaOptions);

        return _instance;
    }

    private KafkaProducerBuilder() { }

    public KafkaProducerBuilder(KafkaOptions kafkaOptions)
    {
        _kafkaOptions = kafkaOptions ?? throw new ArgumentNullException(nameof(kafkaOptions));
    }

    public IProducer<string, string> Build()
    {
        var config = new ClientConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServer
        };

        var producerBuilder = new ProducerBuilder<string, string>(config);

        return producerBuilder.Build();
    }
}
