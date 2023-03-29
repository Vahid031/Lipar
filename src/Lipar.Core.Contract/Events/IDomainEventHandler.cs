using Lipar.Core.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Events;

public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : DomainEvent
{
    Task Handle(TDomainEvent @event, CancellationToken cancellationToken);
}


