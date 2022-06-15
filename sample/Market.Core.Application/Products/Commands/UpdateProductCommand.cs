﻿using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Market.Core.Domain.Products.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
        {
            private readonly IProductCommandRepository repository;

            public UpdateProductCommandHandler(IProductCommandRepository repository)
            {
                this.repository = repository;
            }

            public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken = default)
            {
                var entity = await repository.GetAsync(request.Id);

                entity.Update(request.Name, request.Barcode);

                await repository.CommitAsync();
            }
        }

        public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
        {
            public UpdateProductValidator(ITranslator translator, IProductCommandRepository repository)
            {
                CascadeMode = CascadeMode.Stop;

                RuleFor(m => m.Id)
                    .NotEmpty().WithMessage(translator["not empty"])
                    .Must((entity, prop, context) => repository.Exists(x => prop == x.Id)).WithMessage(translator["not found"]);

                RuleFor(m => m.Name)
                    .NotEmpty().WithMessage(translator["not empty"])
                    .MinimumLength(7).WithMessage(translator["must be {0} character", "7"]);

                RuleFor(m => m.Barcode)
                    .Must(m => int.TryParse(m, out int s)).WithMessage(translator["must be number"])
                    .Must((entity, prop, context) => !repository.Exists(x => x.Barcode.Equals(prop) && entity.Id != x.Id)).WithMessage(translator["same barcode already exist"]);
            }
        }
    }
}