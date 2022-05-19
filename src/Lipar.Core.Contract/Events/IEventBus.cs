using Lipar.Core.Domain.Events;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events
{
    public interface IEventBus
    {
        void Publish<T>(T input);
        void Send(Parcel parcel);
        void Subscribe(string serviceId, string eventName);    }
}
