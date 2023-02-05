using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Products.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Commands;

public class UpdateProductCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Barcode { get; init; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductCommandRepository _repository;

        public UpdateProductCommandHandler(IProductCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(request.Id);

            entity.Update(request.Name, request.Barcode);
            
            _repository.Update(entity);
            await _repository.CommitAsync();
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator(ITranslator translator, IProductCommandRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(m => m.Id)
            .NotEmpty().WithMessage(translator["not empty"])
            .Must((entity, prop, context) => repository.Exists((EntityId)entity.Id)).WithMessage(translator["not found"]);

            RuleFor(m => m.Name)
            .NotEmpty().WithMessage(translator["not empty"])
            .MinimumLength(7).WithMessage(translator["must be {0} character", "7"]);

            RuleFor(m => m.Barcode)
            .Must(m => int.TryParse(m, out int s)).WithMessage(translator["must be number"])
            .Must((entity, prop, context) => !repository.Exists(x => x.Barcode.Equals(prop) && (EntityId)entity.Id != x.Id)).WithMessage(translator["same barcode already exist"]);
        }
    }
}


