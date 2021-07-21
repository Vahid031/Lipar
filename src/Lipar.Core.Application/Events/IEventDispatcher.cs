using Lipar.Core.Domain.Events;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Events
{
    public interface IEventDispatcher
    {
        Task PublishDomainEventAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}
