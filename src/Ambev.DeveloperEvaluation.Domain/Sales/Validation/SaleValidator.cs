using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class SaleValidator
{
    public static IValidator<Sale> Rules { get; } =
        new InlineValidator<Sale>
        {
            r => r.RuleFor(s => s.Number).GreaterThan(0).WithMessage("Sale number must be greater than zero."),
            r => r.RuleFor(s => s.CustomerId).NotEmpty().WithMessage("Customer must be specified."),
            r => r.RuleFor(s => s.BranchId).NotEmpty().WithMessage("Branch must be specified."),
            r => r.RuleForEach(s => s.Items).SetValidator(SaleItemValidator.Rules),
            r => r.RuleForEach(s => s.Discounts).SetValidator(DiscountValidator.Rules),
            r => r.RuleFor(s => s.CreatedAt).NotEmpty().WithMessage("Creation date must be specified."),
            r =>
                r.RuleFor(s => s.Items)
                    .Must(x => x.GroupBy(i => i.ProductId).All(i => i.Count() == 1))
                    .WithMessage("Each sale item must have a distinct product."),
            r =>
                r.RuleFor(s => s.Discounts)
                    .Must(x => x.GroupBy(d => d.Id).All(i => i.Count() == 1))
                    .WithMessage("Each discount must be applied only once."),
        };

    public static ValidationResultDetail Validate(this Sale sale) => new(Rules.Validate(sale));
}
