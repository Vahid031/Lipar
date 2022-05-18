using Lipar.Core.Domain.Events;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events
{
    public interface IEventBus
    {
        Task Publish(IEvent @event);
    }
}
