using Lipar.Core.Contract.Events;
using Lipar.Core.Contract.Utilities;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Products.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Events
{
    public class ProductCreatedEventHandler : IEventHandler<ProductCreated>
    {
        private readonly IEmailService emailService;
        private readonly IJson json;

        public ProductCreatedEventHandler(IEmailService emailService, IJson json)
        {
            this.emailService = emailService;
            this.json = json;
        }
        public async Task Handle(ProductCreated @event, CancellationToken cancellationToken)
        {
            await emailService.SendAsync(new EmailRequest
            {
                Body = json.SerializeObject(@event),
                To = "Vahid031@yahoo.com",
                Subject = "Test"
            });
        }
    }
}
