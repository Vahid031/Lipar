using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Events.RabbitMQ;

public class RabbitMQEventBus : IEventBus
{
    private readonly IJsonService _jsonService;
    private readonly IInBoxEventRepository _inBoxEventRepository;
    private readonly IIntegrationEventDispatcher _eventPublisher;
    private readonly IConnection _connection;
    private readonly LiparOptions _liparOptions;

    public RabbitMQEventBus(LiparOptions liparOptions, IJsonService jsonService, IInBoxEventRepository inBoxEventRepository, IIntegrationEventDispatcher eventPublisher)
    {
        _liparOptions = liparOptions;
        _jsonService = jsonService;
        _inBoxEventRepository = inBoxEventRepository;
        _eventPublisher = eventPublisher;
        var connectionFactory = new ConnectionFactory
        {
            Uri = liparOptions.MessageBus.RabbitMQ.Uri
        };
        _connection = connectionFactory.CreateConnection();

        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(liparOptions.MessageBus.RabbitMQ.ExchangeName,
                                ExchangeType.Topic,
                                liparOptions.MessageBus.RabbitMQ.ExchangeDurable,
                                liparOptions.MessageBus.RabbitMQ.ExchangeAutoDeleted);
    }

    public Task Publish<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent
    {
        string topic = @event.GetType().GetCustomAttribute<EventTopicAttribute>()?.Topic;

        if (string.IsNullOrEmpty(topic))
            throw new ArgumentNullException(nameof(EventTopicAttribute));

        Send(new Parcel
        {
            MessageId = Guid.NewGuid().ToString(),
            MessageBody = _jsonService.SerializeObject(@event),
            MessageName = @event.GetType().Name,
            Route = topic,
            Headers = new Dictionary<string, object>
            {
                ["AccuredOn"] = DateTime.Now.ToString(),
            }
        });

        return Task.CompletedTask;
    }

    private void Send(Parcel parcel)
    {
        var channel = _connection.CreateModel();
        var basicProperties = channel.CreateBasicProperties();
        basicProperties.AppId = _liparOptions.ServiceId;
        basicProperties.CorrelationId = parcel?.CorrelationId;
        basicProperties.MessageId = parcel?.MessageId;
        basicProperties.Headers = parcel.Headers;
        basicProperties.Type = parcel.MessageName;
        channel.BasicPublish(_liparOptions.MessageBus.RabbitMQ.ExchangeName,
        parcel.Route,
        basicProperties,
        Encoding.UTF8.GetBytes(parcel.MessageBody));
    }

    private IntegrationEvent GetEvent(string typeName, string data)
    {
        Type type = Type.GetType(typeName);
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(typeName);
            if (type != null)
                break;
        }

        return (IntegrationEvent)_jsonService.DeserializeObject(data, type);
    }

    public Task Subscribe(Dictionary<string, string> topics, CancellationToken cancellationToken)
    {
        var channel = _connection.CreateModel();
        var consumer = new EventingBasicConsumer(channel);
        var queue = channel.QueueDeclare($"{_liparOptions.ServiceId}", true, false, false);

        foreach (var topic in topics)
        {
            consumer.Received += async (sender, e) =>
            {
                var parcel = e.ToParcel();

                var @event = new InBoxEvent
                {
                    MessageId = parcel.MessageId,
                    OwnerService = e.BasicProperties.AppId,
                    Paload = parcel.MessageBody,
                    ReceivedAt = DateTime.Now,
                    RetryCount = 0,
                    Status = InBoxEventStatus.Scheduled,
                    TypeName = topic.Value
                };

                await _inBoxEventRepository.ReceiveNewEvent(@event);
            };
            channel.QueueBind(queue.QueueName, _liparOptions.MessageBus.RabbitMQ.ExchangeName, topic.Key);
        }

        channel.BasicConsume(queue.QueueName, true, consumer);

        return Task.CompletedTask;
    }
}


