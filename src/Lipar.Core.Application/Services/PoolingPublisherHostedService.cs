using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Services;

public class PoolingPublisherHostedService : BackgroundService
{
    private readonly LiparOptions _liparOptions;
    private readonly IOutBoxEventRepository _outBoxEventRepository;
    private readonly IEventBus _eventBus;

    private readonly IJsonService _jsonService;
    private readonly IServiceCollection _services;
    private Timer _timer;

    public PoolingPublisherHostedService(IServiceProvider serviceProvider, IServiceCollection services)
    {
        _liparOptions = serviceProvider.GetRequiredService<LiparOptions>();
        _outBoxEventRepository = serviceProvider.GetRequiredService<IOutBoxEventRepository>();
        _eventBus = serviceProvider.GetRequiredService<IEventBus>();
        _jsonService = serviceProvider.GetRequiredService<IJsonService>();
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(SendOutBoxItems, null, TimeSpan.Zero, TimeSpan.FromSeconds(_liparOptions.PoolingPublisher.SendOutBoxInterval));
        await SubscribeEvents(stoppingToken);
    }

    private void SendOutBoxItems(object state)
    {
        _timer.Change(Timeout.Infinite, 0);

        var outboxItems = _outBoxEventRepository.GetOutBoxEventItemsForPublish(_liparOptions.PoolingPublisher.SendOutBoxCount).GetAwaiter().GetResult();

        foreach (var item in outboxItems)
        {
            // Raize event inside the application
            IEvent @event = GetEvent(item.EventTypeName, item.EventPayload);

            // Sending on Message Broker
            _eventBus.Publish(@event).GetAwaiter().GetResult();

            item.IsProcessed = true;
        }

        // Done
        _outBoxEventRepository.MarkAsRead(outboxItems).GetAwaiter().GetResult();
        _timer.Change(0, _liparOptions.PoolingPublisher.SendOutBoxInterval);

    }



    private async Task SubscribeEvents(CancellationToken cancellationToken)
    {
        var events = _services
            .Where(m => m.ServiceType.IsGenericType && m.ServiceType.GetGenericTypeDefinition() == typeof(IEventHandler<>))
            .Select(m => new
            {
                Type = GetType(m.ServiceType.GetGenericArguments()[0].FullName),
                Topic = (m.ImplementationType as TypeInfo).GetDeclaredMethod("Handle").GetCustomAttribute<EventTopicAttribute>().Topic
            })
            .ToList();

        foreach (var @event in events)
        {
            await Task.Run(async () =>
              {
                  await _eventBus.Subscribe(@event.Topic, @event.Type, cancellationToken);
              }, cancellationToken);
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

    private Type GetType(string typeName)
    {
        Type type = Type.GetType(typeName);
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(typeName);
            if (type != null)
                break;
        }

        return type;
    }
}


