﻿using Lipar.Core.Application.Events;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Events.OutboxEvent;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Infrastructure.Tools.Utilities.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Events.PoolingPublisher
{
    public class PoolingPublisherHostedService : IHostedService
    {
        private readonly LiparOptions _liparOptions;
        private readonly IEventPublisher _publisher;

        private readonly IOutBoxEventRepository _outBoxEventRepository;
        private readonly IJson _json;

        //private readonly IMessageBus _messageBus;
        private Timer _timer;

        public PoolingPublisherHostedService(LiparOptions liparOptions,
            IEventPublisher publisher,
            IOutBoxEventRepository outBoxEventRepository,
            IJson json)
        {
            _liparOptions = liparOptions;
            _publisher = publisher;
            _outBoxEventRepository = outBoxEventRepository;
            _json = json;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendOutBoxItems, null, TimeSpan.Zero, TimeSpan.FromSeconds(_liparOptions.PoolingPublisher.SendOutBoxInterval));
            return Task.CompletedTask;
        }

        private void SendOutBoxItems(object state)
        {
            _timer.Change(Timeout.Infinite, 0);


            return;

            var outboxItems = _outBoxEventRepository.GetOutBoxEventItemsForPublish(_liparOptions.PoolingPublisher.SendOutBoxCount);

            foreach (var item in outboxItems)
            {
                IEvent @event = GetEvent(item.EventTypeName, item.EventPayload);
                _publisher.Raise(@event);

                item.IsProcessed = true;
            }
            _outBoxEventRepository.MarkAsRead(outboxItems);
            _timer.Change(0, _liparOptions.PoolingPublisher.SendOutBoxInterval);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public IEvent GetEvent(string typeName, string data)
        {
            Type type = Type.GetType(typeName);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(typeName);
                if (type != null)
                    break;
            }

            return (IEvent)_json.DeserializeObject(data, type);
        }
    }
}