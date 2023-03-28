using Lipar.Core.Contract.Events;
using System.Threading;
using System.Threading.Tasks;
using Lipar.Core.Domain.Events;
using Market.Core.Domain.Categories.Events;

namespace Market.Core.Application.Categories.Events;

public class CategoryCreatedEventHandler : IDomainEventHandler<CategoryCreated>
{

    public Task Handle(CategoryCreated @event, CancellationToken cancellationToken)
    {
        //await _emailService.SendAsync(new EmailRequest
        //{
        //    Body = _jsonService.SerializeObject(@event),
        //    To = "Vahid031@yahoo.com",
        //    Subject = "Test"
        //});

        return Task.CompletedTask;
    }
}


