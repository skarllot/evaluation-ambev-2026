using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Services;

public class TenItemsAutomaticDiscount(ILogger<TenItemsAutomaticDiscount> logger, IDiscountRepository repository)
    : IAutomaticDiscount
{
    private static readonly DiscountCode _discountCode = DiscountCode.Auto10Items;

    public async ValueTask TryApply(Sale sale, CancellationToken cancellationToken)
    {
        if (!CanApply(sale))
        {
            return;
        }

        var discount = await repository.GetDiscount(_discountCode, cancellationToken);
        if (discount is null)
        {
            throw new InvalidOperationException($"The discount '{_discountCode.Value}' was not found.");
        }

        sale.Discounts.Add(discount);
        logger.LogInformation("The discount '{DiscountCode}' was applied to sale", discount.Code.Value);
    }

    private static bool CanApply(Sale sale) => sale.Items.Any(i => i.Quantity.Value is >= 10 and <= 20);
}
