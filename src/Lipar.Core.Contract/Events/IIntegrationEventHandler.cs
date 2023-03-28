using Lipar.Core.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken);
}


