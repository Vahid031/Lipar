using Lipar.Core.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IIntegrationEventPublisher
{
    Task Raise<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default) where TIntegrationEvent : IntegrationEvent;
}

