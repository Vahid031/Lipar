using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Products.Events;
using System.Threading;
using System.Threading.Tasks;
using Lipar.Core.Domain.Events;

namespace Market.Core.Application.Products.Events;

public class ProductCreatedEventHandler : IDomainEventHandler<ProductCreated>
{
    private readonly IEmailService _emailService;
    private readonly IJsonService _jsonService;

    public ProductCreatedEventHandler(IEmailService emailService, IJsonService jsonService)
    {
        _emailService = emailService;
        _jsonService = jsonService;
    }

    [EventTopic("MarketService.ProductCreated")]
    public Task Handle(ProductCreated @event, CancellationToken cancellationToken)
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


