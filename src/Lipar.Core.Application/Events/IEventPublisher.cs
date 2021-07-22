using Lipar.Core.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Events
{
    public interface IEventPublisher
    {
        Task Raise<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent;
    }
}
