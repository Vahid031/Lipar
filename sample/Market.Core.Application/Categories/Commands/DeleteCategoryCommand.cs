using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Market.Core.Domain.Categories.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Categories.Commands;

public class DeleteCategoryCommand : IRequest
{
public Guid Id { get; init; }
    
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryCommandRepository repository;
        
        public DeleteCategoryCommandHandler(ICategoryCommandRepository repository)
        {
            this.repository = repository;
        }
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repository.GetAsync(request.Id);
            
            entity.Delete();
            
            repository.Delete(entity);
            await repository.CommitAsync();
        }
    }
    
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryValidator(ITranslator translator, ICategoryCommandRepository repository)
        {
            RuleFor(c => c.Id)
            .NotEmpty().WithMessage(m => translator["not empty"])
            .NotNull().WithMessage(m => translator["not empty"])
            .Must((entity, prop, context) => repository.Exists(prop)).WithMessage(m => translator["not found"])
            .Must((entity, prop, context) => repository.Exists(m => m.ParentId != null && m.ParentId.Value == prop)).WithMessage(m => translator["entity has child, cannot delete it"]);
            
        }
    }
}


