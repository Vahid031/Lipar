using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Products.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Commands;

public class DeleteProductCommand : IRequest
{
public Guid Id { get; init; }
    
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductCommandRepository repository;
        
        public DeleteProductCommandHandler(IProductCommandRepository repository)
        {
            this.repository = repository;
        }
        
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repository.GetAsync(request.Id);
            entity.Delete();
            
            repository.Delete(entity);
            await repository.CommitAsync();
        }
    }
    
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator(ITranslator translator, IProductCommandRepository repository)
        {
            CascadeMode = CascadeMode.Stop;
            
            RuleFor(m => m.Id)
            .NotEmpty().WithMessage(translator["not empty"])
            .Must((entity, prop, context) => repository.Exists(x => prop == x.Id.Value)).WithMessage(translator["not found"]);
        }
    }
    
}


