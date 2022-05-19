namespace Lipar.Infrastructure.Tools.Utilities.Configurations
{
    public class LiparOptions
    {
        public string ServiceId { get; set; }
        public SwaggerOptions Swagger { get; set; }
        public OutBoxEventOptions OutBoxEvent { get; set; }
        public PoolingPublisherOptions PoolingPublisher { get; set; }
        public ChangesInterceptionOptions EntityChangesInterceptor { get; set; }
        public MessageBusOptions MessageBus { get; set; }

    }

    public class MessageBusOptions
    {
        public RabbitMQOptions RabbitMQ { get; set; }
        public EventOptions[] Events { get; set; }
    }

    public class EventOptions
    {
        public string ServiceId { get; set; }
        public string EventName { get; set; }
        public string MapToClass { get; set; }
    }
}
