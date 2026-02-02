using Ambev.DeveloperEvaluation.Domain.Products.Validation;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class SaleItemValidator
{
    public static IValidator<SaleItem> Rules { get; } =
        new InlineValidator<SaleItem>
        {
            r => r.RuleFor(si => si.ProductId).NotEmpty().WithMessage("Product is required."),
            r => r.RuleFor(si => si.Quantity).SetValidator(ProductQuantityValidator.Rules),
            r => r.RuleFor(si => si.UnitPrice).SetValidator(ProductPriceValidator.Rules),
        };

    public static void ValidateAndThrow(this SaleItem saleItem) => Rules.ValidateAndThrow(saleItem);
}
