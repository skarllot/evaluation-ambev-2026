using System.Collections.Immutable;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Services;

public class SaleDiscountService(
    IDiscountRepository discountRepository,
    IEnumerable<IAutomaticDiscount> automaticDiscounts
) : ISaleDiscountService
{
    public async Task ApplyDiscounts(Sale sale, IImmutableList<Guid> discountIds, CancellationToken cancellationToken)
    {
        var discounts = await discountRepository
            .GetDiscounts(discountIds, includeAutomatic: false)
            .ToDictionaryAsync(x => x.Id, cancellationToken: cancellationToken);

        if (discountIds.Any(d => !discounts.ContainsKey(d)))
        {
            throw new ValidationException(
                $"Invalid discount requested: {string.Join(", ", discountIds.Where(d => !discounts.ContainsKey(d)))}."
            );
        }

        if (discountIds.Count > 0 && sale.Items.Sum(i => i.Quantity.Value) < 4)
        {
            throw new ValidationException($"Purchases below 4 items cannot have a discount.");
        }

        sale.Discounts.Clear();
        sale.Discounts.AddRange(discountIds.Select(d => discounts[d]));

        foreach (var automaticDiscount in automaticDiscounts)
        {
            await automaticDiscount.TryApply(sale, cancellationToken);
        }
    }
}
