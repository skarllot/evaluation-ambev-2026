using Ambev.DeveloperEvaluation.Domain.Sales.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Services;

public interface IAutomaticDiscount
{
    ValueTask TryApply(Sale sale, CancellationToken cancellationToken);
}