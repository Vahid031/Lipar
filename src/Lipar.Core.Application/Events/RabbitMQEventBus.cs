using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Events
{
    public class RabbitMQEventBus : IEventBus
    {
        public Task Publish(IEvent @event)
        {
           return Task.CompletedTask;
        }
    }
}
