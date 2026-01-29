using Ambev.DeveloperEvaluation.Domain.Sales.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Repositories;

public interface IDiscountRepository
{
    public IAsyncEnumerable<Discount> GetDiscounts(IEnumerable<Guid> discountIds, CancellationToken cancellationToken);
}
