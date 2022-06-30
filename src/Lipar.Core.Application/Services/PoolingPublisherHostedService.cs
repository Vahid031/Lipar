using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Services;

public class PoolingPublisherHostedService : IHostedService
{
    private readonly LiparOptions _liparOptions;
    private readonly IOutBoxEventRepository _outBoxEventRepository;
    private readonly IEventBus _eventBus;
    private readonly IEventPublisher _eventPublisher;
    private readonly IJsonService _jsonService;
    private Timer _timer;
    
    public PoolingPublisherHostedService(LiparOptions liparOptions,
    IOutBoxEventRepository outBoxEventRepository,
    IEventBus eventBus,
    IEventPublisher eventPublisher,
    IJsonService jsonService)
    {
        _liparOptions = liparOptions;
        _outBoxEventRepository = outBoxEventRepository;
        _eventBus = eventBus;
        _eventPublisher = eventPublisher;
        _jsonService = jsonService;
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
            // All events will publish exept current service
            foreach (var @event in _liparOptions.MessageBus.Events.Where(m => m.ServiceId.Equals(_liparOptions.ServiceId)).ToList())
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
            // Raize event inside the application
            IEvent @event = GetEvent(item.EventTypeName, item.EventPayload);
            _eventPublisher.Raise(@event);
            
            // Sending on Message Broker
            _eventBus.Publish(@event);
            
            //_eventBus.Send(new Parcel
            //{
                //    CorrelationId = item.AggregateId,
                //    MessageBody = item.EventPayload,
                //    MessageId = item.Id.ToString(),
                //    MessageName = item.EventName,
                //    Headers = new Dictionary<string, object>
                //    {
                    //        ["AccuredByUserId"] = item.AccuredByUserId,
                    //        ["AccuredOn"] = item.AccuredOn.ToString(),
                    //        ["AggregateName"] = item.AggregateName,
                    //        ["AggregateTypeName"] = item.AggregateTypeName,
                    //        ["EventTypeName"] = item.EventTypeName,
                //    }
            //});
            
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


