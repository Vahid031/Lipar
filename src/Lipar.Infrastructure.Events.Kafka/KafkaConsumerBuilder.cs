using Confluent.Kafka;
using Lipar.Infrastructure.Tools.Utilities.Configurations;

namespace Lipar.Infrastructure.Events.Kafka;

public class KafkaConsumerBuilder
{
    private static KafkaConsumerBuilder _instance;
    private readonly KafkaOptions _kafkaOptions;

    public static KafkaConsumerBuilder CreateFactoty(KafkaOptions kafkaOptions)
    {
        if (_instance == null)
            _instance = new KafkaConsumerBuilder(kafkaOptions);

        return _instance;
    }
    private KafkaConsumerBuilder() { }
    private KafkaConsumerBuilder(KafkaOptions kafkaOptions)
    {
        _kafkaOptions = kafkaOptions;
    }

    public IConsumer<string, string> Build()
    {
        var config = new ClientConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServer
        };

        var consumerConfig = new ConsumerConfig(config)
        {
            GroupId = _kafkaOptions.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig);

        return consumerBuilder.Build();
    }
}