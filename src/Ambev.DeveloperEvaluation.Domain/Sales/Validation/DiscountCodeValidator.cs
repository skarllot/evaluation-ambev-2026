using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validation;

public static class DiscountCodeValidator
{
    public static IValidator<DiscountCode> Rules { get; } =
        new InlineValidator<DiscountCode>
        {
            r => r.RuleFor(q => q.Value).NotEmpty().WithMessage("The discount code must not be empty."),
        };

    public static void ValidateAndThrow(this DiscountCode discountCode) => Rules.ValidateAndThrow(discountCode);
}
