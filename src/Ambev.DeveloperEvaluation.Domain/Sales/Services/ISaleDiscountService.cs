using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using System.Collections.Immutable;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Services;

public interface ISaleDiscountService
{
    Task ApplyDiscounts(Sale sale, IImmutableList<Guid> discountIds, CancellationToken cancellationToken);
}