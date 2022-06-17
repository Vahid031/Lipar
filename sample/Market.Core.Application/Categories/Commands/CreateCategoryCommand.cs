using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Categories.Contracts;
using Market.Core.Domain.Categories.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest
    {
        public string Name { get; init; }
        public Guid? ParentId { get; init; }

        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
        {
            private readonly ICategoryCommandRepository repository;

            public CreateCategoryCommandHandler(ICategoryCommandRepository repository)
            {
                this.repository = repository;
            }
            public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken = default)
            {
                var entity = new Category(Guid.NewGuid(), request.Name, request.ParentId);

                await repository.InsertAsync(entity);
                await repository.CommitAsync();
            }
        }

        public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
        {
            public CreateCategoryValidator(ITranslator translator, ICategoryCommandRepository repository)
            {
                RuleFor(c => c.Name)
                    .NotEmpty().WithMessage(m => translator["not empty"])
                    .NotNull().WithMessage(m => translator["not empty"]);

                RuleFor(c => c.ParentId)
                    .Must((entity, prop, context) => prop is null || repository.Exists(c => c.Id == EntityId.FromGuid(prop))).WithMessage(m => translator["ParentId not found"]);

            }
        }
    }
}
