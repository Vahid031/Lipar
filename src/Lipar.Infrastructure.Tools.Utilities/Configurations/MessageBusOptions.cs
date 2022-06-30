namespace Lipar.Infrastructure.Tools.Utilities.Configurations;

public class MessageBusOptions
{
public RabbitMQOptions RabbitMQ { get; set; }
public EventOptions[] Events { get; set; }
}


