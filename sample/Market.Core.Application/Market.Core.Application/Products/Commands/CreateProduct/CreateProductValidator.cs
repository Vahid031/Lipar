using FluentValidation;

namespace Market.Core.Application.Products.Commands.CreateProduct
{
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
