using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public static class CreateSaleRequestValidator
{
    private static readonly IValidator<CreateSaleItemDto> _saleItemValidator = new InlineValidator<CreateSaleItemDto>
    {
        r => r.RuleFor(i => i.ProductId).NotEmpty(),
        r => r.RuleFor(i => i.Quantity).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20),
        r => r.RuleFor(i => i.UnitPrice).GreaterThan(0).PrecisionScale(8, 2, ignoreTrailingZeros: true),
    };

    public static IValidator<CreateSaleRequest> Rules { get; } =
        new InlineValidator<CreateSaleRequest>
        {
            r => r.RuleFor(c => c.CustomerId).NotEmpty(),
            r => r.RuleFor(c => c.BranchId).NotEmpty(),
            r => r.RuleForEach(c => c.Items).SetValidator(_saleItemValidator),
            r => r.RuleForEach(c => c.Discounts).NotEmpty(),
        };

    public static ValidationResult Validate(this CreateSaleRequest request) => Rules.Validate(request);
}
