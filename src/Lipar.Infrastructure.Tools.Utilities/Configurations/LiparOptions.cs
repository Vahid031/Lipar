namespace Lipar.Infrastructure.Tools.Utilities.Configurations;

public class LiparOptions
{
    public string ServiceId { get; set; }
    public SwaggerOptions Swagger { get; set; }
    public OutBoxEventOptions OutBoxEvent { get; set; }
    public InBoxEventOptions InBoxEvent { get; set; }
    public PoolingPublisherOptions PoolingPublisher { get; set; }
    public ChangesInterceptionOptions EntityChangesInterception { get; set; }
    public MessageBusOptions MessageBus { get; set; }
    public MailOptions Mail { get; set; }
    public TranslationOptions Translation { get; set; }
    public MongoDbOptions MongoDb { get; set; }

}


