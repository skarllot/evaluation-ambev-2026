using Ambev.DeveloperEvaluation.Domain.Sales.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Repositories;

public interface ISaleRepository
{
    Task Create(Sale sale, CancellationToken cancellationToken = default);
}