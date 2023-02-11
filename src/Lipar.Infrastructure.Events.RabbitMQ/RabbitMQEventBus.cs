using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lipar.Infrastructure.Events.RabbitMQ;

public class RabbitMQEventBus : IEventBus
{
    private readonly IJsonService _jsonService;
    private readonly IInBoxEventRepository _inBoxEventRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IConnection _connection;
    private readonly LiparOptions _liparOptions;
    private readonly Dictionary<string, string> _messageTypeMap;

    public RabbitMQEventBus(LiparOptions liparOptions, IJsonService jsonService, IInBoxEventRepository inBoxEventRepository, IEventPublisher eventPublisher)
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
            Route = topic,
            Headers = new Dictionary<string, object>
            {
                ["AccuredOn"] = DateTime.Now.ToString(),
            }
        });
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

    private void Consumer_EventReceived(object sender, BasicDeliverEventArgs e)
    {
        var parcel = e.ToParcel();

        if (_inBoxEventRepository.AllowReceive(parcel.MessageId, e.BasicProperties.AppId))
        {
            var mapToClass = _messageTypeMap[parcel.Route];
            var @event = GetEvent(mapToClass, parcel.MessageBody);
            _eventPublisher.Raise(@event);
            _inBoxEventRepository.Receive(parcel.MessageId, e.BasicProperties.AppId);
        }
    }

    private IEvent GetEvent(string typeName, string data)
    {
        Type type = Type.GetType(typeName);
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(typeName);
            if (type != null)
                break;
        }

        return (IEvent)_jsonService.DeserializeObject(data, type);
    }

    public void Subscribe<TEvent>(string topic) where TEvent : IEvent
    {
        Subscribe(topic, typeof(TEvent));
    }

    public void Subscribe(string topic, Type type)
    {
        var channel = _connection.CreateModel();
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, e) =>
    {
        var parcel = e.ToParcel();

        if (_inBoxEventRepository.AllowReceive(parcel.MessageId, e.BasicProperties.AppId))
        {
            var @event = (IEvent)_jsonService.DeserializeObject(parcel.MessageBody, type); ;
            _eventPublisher.Raise(@event);
            _inBoxEventRepository.Receive(parcel.MessageId, e.BasicProperties.AppId);
        }
    };
        var queue = channel.QueueDeclare($"{_liparOptions.ServiceId}", true, false, false);

        channel.QueueBind(queue.QueueName, _liparOptions.MessageBus.RabbitMQ.ExchangeName, topic);
        channel.BasicConsume(queue.QueueName, true, consumer);
    }
}


