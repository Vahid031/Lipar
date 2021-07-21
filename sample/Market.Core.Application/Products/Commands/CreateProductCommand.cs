using FluentValidation;
using Lipar.Core.Application.Common;
using Market.Core.Domain.Products.Entities;
using Market.Core.Domain.Products.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Commands
{
    public class CreateProductCommand : IRequest
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
        {
            private readonly IProductCommandRepository repository;

            public CreateProductCommandHandler(IProductCommandRepository repository)
            {
                this.repository = repository;
            }

            public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken = default)
            {
                var entity = new Product(Guid.NewGuid(), request.Name, request.Barcode);

                await repository.InsertAsync(entity);
                await repository.CommitAsync();
            }
        }

        public class CreateProductValidator : AbstractValidator<CreateProductCommand>
        {
            public CreateProductValidator()
            {
                RuleFor(m => m.Name)
                    .NotEmpty()
                    .MinimumLength(7);

                RuleFor(m => m.Barcode)
                    .Must(m => int.TryParse(m, out int s));
            }
        }
    }
}
