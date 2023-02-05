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
        private readonly IProductCommandRepository _repository;

        public DeleteProductCommandHandler(IProductCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(request.Id);
            entity.Delete();

            _repository.Delete(entity);
            await _repository.CommitAsync();
        }
    }

    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator(ITranslator translator, IProductCommandRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(m => m.Id)
            .NotEmpty().WithMessage(translator["not empty"])
            .Must((entity, prop, context) => repository.Exists(x => (EntityId)prop == x.Id)).WithMessage(translator["not found"]);
        }
    }

}


