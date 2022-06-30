using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Market.Core.Domain.Products.Contracts;
using Market.Core.Domain.Products.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Commands;

public partial class CreateProductCommand : IRequest
{
public string Name { get; init; }
public string Barcode { get; init; }
    
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
        public CreateProductValidator(ITranslator translator, IProductCommandRepository repository)
        {
            CascadeMode = CascadeMode.Stop;
            
            RuleFor(m => m.Name)
            .NotEmpty().WithMessage(translator["not empty"])
        .MinimumLength(7).WithMessage(translator["must be {0} character", "7"]);
            
            RuleFor(m => m.Barcode)
            .Must(m => int.TryParse(m, out int s)).WithMessage(translator["must be number"])
            //.Must((entity, prop, context) => !repository.Exists(x => x.Barcode.Equals(prop))).WithMessage(translator["same barcode already exist"])
            ;
        }
    }
}


