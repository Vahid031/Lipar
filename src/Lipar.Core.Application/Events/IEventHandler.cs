using Lipar.Core.Domain.Events;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Events
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}
