using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class ProductQuantityValidator
{
    public static IValidator<ProductQuantity> Rules { get; } =
        new InlineValidator<ProductQuantity>
        {
            r =>
                r.RuleFor(q => q.Value)
                    .GreaterThanOrEqualTo(1)
                    .WithMessage("Product quantity must be greater than or equal to 1.")
                    .LessThanOrEqualTo(20)
                    .WithMessage("Product quantity must be less than or equal to 20."),
        };

    public static void ValidateAndThrow(this ProductQuantity productQuantity) =>
        Rules.ValidateAndThrow(productQuantity);
}
