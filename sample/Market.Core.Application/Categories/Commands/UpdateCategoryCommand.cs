using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Market.Core.Domain.Categories.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest
{
public Guid Id { get; init; }
public string Name { get; init; }
public Guid? ParentId { get; init; }
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryCommandRepository repository;
        
        public UpdateCategoryCommandHandler(ICategoryCommandRepository repository)
        {
            this.repository = repository;
        }
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await repository.GetAsync(request.Id);
            
            entity.Update(request.Name, request.ParentId);
            
            await repository.CommitAsync();
        }
    }
    
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator(ITranslator translator, ICategoryCommandRepository repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage(m => translator["not empty"])
            .NotNull().WithMessage(m => translator["not empty"]);
            
            RuleFor(c => c.ParentId)
            .NotEmpty().WithMessage(m => translator["not empty"])
            .Must((entity, prop, context) => repository.Exists(c => c.Id.Value == prop)).WithMessage(m => translator["ParentId not found"]);
            
        }
    }
}


