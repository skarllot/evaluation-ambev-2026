using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Repositories;

public interface IDiscountRepository
{
    public IAsyncEnumerable<Discount> GetDiscounts(IEnumerable<Guid> discountIds, bool includeAutomatic);

    public Task<Discount?> GetDiscount(DiscountCode code, CancellationToken cancellationToken);
}
