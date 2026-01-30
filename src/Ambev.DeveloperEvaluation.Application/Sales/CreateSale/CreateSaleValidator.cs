using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public static class CreateSaleValidator
{
    private static readonly IValidator<CreateSaleItem> _saleItemValidator = new InlineValidator<CreateSaleItem>
    {
        r => r.RuleFor(i => i.ProductId).NotEmpty(),
        r => r.RuleFor(i => i.Quantity).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20),
        r => r.RuleFor(i => i.UnitPrice).GreaterThan(0).PrecisionScale(8, 2, ignoreTrailingZeros: true),
    };

    public static IValidator<CreateSaleCommand> Rules { get; } =
        new InlineValidator<CreateSaleCommand>
        {
            r => r.RuleFor(c => c.CustomerId).NotEmpty(),
            r => r.RuleFor(c => c.BranchId).NotEmpty(),
            r => r.RuleForEach(c => c.Items).SetValidator(_saleItemValidator),
            r => r.RuleForEach(c => c.Discounts).NotEmpty(),
        };

    public static void ValidateAndThrow(this CreateSaleCommand command) => Rules.ValidateAndThrow(command);
}
