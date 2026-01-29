using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class DiscountValidator
{
    public static IValidator<Discount> Rules { get; } =
        new InlineValidator<Discount>
        {
            r => r.RuleFor(si => si.Code).SetValidator(DiscountCodeValidator.Rules),
            r =>
                r.RuleFor(si => si)
                    .Must(si => si.Multiplier.HasValue || si.Amount.HasValue)
                    .WithMessage("Discount must specify either a percentage multiplier or a fixed amount value."),
            r =>
                r.RuleFor(si => si.Multiplier)
                    .GreaterThan(0)
                    .WithMessage("Discount multiplier must be greater than 0 (0%).")
                    .LessThan(1)
                    .WithMessage(
                        "Discount multiplier must be less than 1 (100%), otherwise it would result in a free item."
                    )
                    .PrecisionScale(4, 4, ignoreTrailingZeros: true)
                    .WithMessage("Discount multiplier precision cannot exceed 4 decimal places.")
                    .When(si => si.Amount is null),
            r =>
                r.RuleFor(si => si.Amount)
                    .GreaterThan(0)
                    .WithMessage("Discount amount must be greater than 0.00.")
                    .LessThan(100_000m)
                    .WithMessage("Discount amount cannot exceed 99,999.99.")
                    .PrecisionScale(7, 2, ignoreTrailingZeros: true)
                    .WithMessage("Discount amount cannot have more than 2 decimal places.")
                    .When(si => si.Multiplier is null),
        };

    public static ValidationResultDetail Validate(this Discount discount) => new(Rules.Validate(discount));
}
