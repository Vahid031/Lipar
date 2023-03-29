using Lipar.Core.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IDomainEventPublisher
{
    Task Raise<TDomainEvent>(TDomainEvent @event, CancellationToken cancellationToken = default) where TDomainEvent : DomainEvent;
}

