using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository(DefaultContext dbContext) : ISaleRepository
{
    public async Task Create(Sale sale, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<Sale>().AddAsync(sale, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
