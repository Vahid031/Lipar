using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Products.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Events;

public class ProductCreatedEventHandler : IEventHandler<ProductCreated>
{
    private readonly IEmailService emailService;
    private readonly IJsonService jsonService;
    
    public ProductCreatedEventHandler(IEmailService emailService, IJsonService jsonService)
    {
        this.emailService = emailService;
        this.jsonService = jsonService;
    }
    public async Task Handle(ProductCreated @event, CancellationToken cancellationToken)
    {
        await emailService.SendAsync(new EmailRequest
        {
            Body = jsonService.SerializeObject(@event),
            To = "Vahid031@yahoo.com",
            Subject = "Test"
        });
    }
}


