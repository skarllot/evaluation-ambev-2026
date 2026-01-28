using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Products.Validation;

public static class ProductPriceValidator
{
    public static IValidator<ProductPrice> Rules { get; } =
        new InlineValidator<ProductPrice>
        {
            r => r.RuleFor(q => q.Amount).GreaterThan(0).WithMessage("Product price must be greater than zero."),
        };

    public static ValidationResultDetail Validate(this ProductPrice productPrice) => new(Rules.Validate(productPrice));
}
