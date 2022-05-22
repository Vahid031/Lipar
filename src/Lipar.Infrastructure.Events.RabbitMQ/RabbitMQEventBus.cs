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
using System.Text;

namespace Lipar.Infrastructure.Events.RabbitMQ
{
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


            _messageTypeMap = new Dictionary<string, string>();
            if (_liparOptions?.MessageBus?.Events?.Any() == true)
            {
                foreach (var @event in _liparOptions.MessageBus.Events)
                {
                    _messageTypeMap.Add($"{@event.ServiceId}.{@event.EventName}", @event.MapToClass);
                }
            }
        }

        public void Publish<T>(T input)
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

        public void Send(Parcel parcel)
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

        public void Subscribe(string serviceId, string eventName)
        {
            var route = $"{serviceId}.{eventName}";

            var channel = _connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_EventReceived;
            var queue = channel.QueueDeclare($"{ _liparOptions.ServiceId}", true, false, false);

            channel.QueueBind(queue.QueueName, _liparOptions.MessageBus.RabbitMQ.ExchangeName, route);
            channel.BasicConsume(queue.QueueName, true, consumer);
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
    }
}
