using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class SaleValidator
{
    public static IValidator<Sale> Rules { get; } =
        new InlineValidator<Sale>
        {
            r =>
                r.RuleFor(s => s.Number)
                    .GreaterThan(0)
                    .WithMessage("Sale number must be an integer greater than zero."),
            r => r.RuleFor(s => s.CustomerId).NotEmpty().WithMessage("Customer is required."),
            r => r.RuleFor(s => s.BranchId).NotEmpty().WithMessage("Branch is required."),
            r => r.RuleForEach(s => s.Items).SetValidator(SaleItemValidator.Rules),
            r => r.RuleForEach(s => s.Discounts).SetValidator(DiscountValidator.Rules),
            r => r.RuleFor(s => s.CreatedAt).NotEmpty().WithMessage("Sale creation date is required."),
            r =>
                r.RuleFor(s => s.Items)
                    .Must(x => x.GroupBy(i => i.ProductId).All(i => i.Count() == 1))
                    .WithMessage("Each product can only appear once per sale, adjust quantities instead."),
            r =>
                r.RuleFor(s => s.Discounts)
                    .Must(x => x.GroupBy(d => d.Id).All(i => i.Count() == 1))
                    .WithMessage("Each discount can only be applied once per sale."),
        };

    public static ValidationResultDetail Validate(this Sale sale) => new(Rules.Validate(sale));
}
