using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Products.Validation;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class SaleItemValidator
{
    public static IValidator<SaleItem> Rules { get; } =
        new InlineValidator<SaleItem>
        {
            r => r.RuleFor(si => si.ProductId).NotEmpty().WithMessage("Product identity is required."),
            r => r.RuleFor(si => si.Quantity).SetValidator(ProductQuantityValidator.Rules),
            r => r.RuleFor(si => si.UnitPrice).SetValidator(ProductPriceValidator.Rules),
        };

    public static ValidationResultDetail Validate(this SaleItem saleItem) => new(Rules.Validate(saleItem));
}
