namespace Lipar.Infrastructure.Tools.Utilities.Configurations;

public class MessageBusOptions
{
    public string TypeName { get; init; }
    public RabbitMQOptions RabbitMQ { get; set; }
    public KafkaOptions Kafka { get; set; }
}


