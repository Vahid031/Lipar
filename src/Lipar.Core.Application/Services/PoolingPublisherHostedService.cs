using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Services
{
    public class PoolingPublisherHostedService : IHostedService
    {
        private readonly LiparOptions _liparOptions;
        private readonly IOutBoxEventRepository _outBoxEventRepository;
        private readonly IEventBus _eventBus;
        private Timer _timer;

        public PoolingPublisherHostedService(LiparOptions liparOptions,
            IOutBoxEventRepository outBoxEventRepository,
            IEventBus eventBus)
        {
            _liparOptions = liparOptions;
            _outBoxEventRepository = outBoxEventRepository;
            _eventBus = eventBus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SubscribeEvents();
            _timer = new Timer(SendOutBoxItems, null, TimeSpan.Zero, TimeSpan.FromSeconds(_liparOptions.PoolingPublisher.SendOutBoxInterval));
        }
        private Task SubscribeEvents()
        {
            if (_liparOptions?.MessageBus?.Events?.Any() == true)
            {
                foreach (var @event in _liparOptions.MessageBus.Events)
                {
                    _eventBus.Subscribe(@event.ServiceId, @event.EventName);
                }
            }
            return Task.CompletedTask;
        }

        private void SendOutBoxItems(object state)
        {
            _timer.Change(Timeout.Infinite, 0);

            var outboxItems = _outBoxEventRepository.GetOutBoxEventItemsForPublish(_liparOptions.PoolingPublisher.SendOutBoxCount);

            foreach (var item in outboxItems)
            {

                // Sending on Message Broker
                _eventBus.Send(new Parcel
                {
                    CorrelationId = item.AggregateId,
                    MessageBody = item.EventPayload,
                    MessageId = item.Id.ToString(),
                    MessageName = item.EventName,
                    Route = $"{_liparOptions.ServiceId}.{item.EventName}",
                    Headers = new Dictionary<string, object>
                    {
                        ["AccuredByUserId"] = item.AccuredByUserId,
                        ["AccuredOn"] = item.AccuredOn.ToString(),
                        ["AggregateName"] = item.AggregateName,
                        ["AggregateTypeName"] = item.AggregateTypeName,
                        ["EventTypeName"] = item.EventTypeName,
                    }
                });

                // Raize event inside the application
                //IEvent @event = GetEvent(item.EventTypeName, item.EventPayload);
                //_publisher.Raise(@event);

                item.IsProcessed = true;
            }

            // Done
            _outBoxEventRepository.MarkAsRead(outboxItems);
            _timer.Change(0, _liparOptions.PoolingPublisher.SendOutBoxInterval);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
