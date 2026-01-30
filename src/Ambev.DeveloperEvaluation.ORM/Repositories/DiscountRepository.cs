using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class DiscountRepository(DefaultContext dbContext) : IDiscountRepository
{
    public IAsyncEnumerable<Discount> GetDiscounts(IEnumerable<Guid> discountIds, bool includeAutomatic)
    {
        return dbContext
            .Set<Discount>()
            .Where(d => (includeAutomatic || !d.IsAutomatic) && discountIds.Contains(d.Id))
            .AsAsyncEnumerable();
    }

    public Task<Discount?> GetDiscount(DiscountCode code, CancellationToken cancellationToken)
    {
        return dbContext.Set<Discount>().Where(d => d.Code == code).FirstOrDefaultAsync(cancellationToken);
    }
}
