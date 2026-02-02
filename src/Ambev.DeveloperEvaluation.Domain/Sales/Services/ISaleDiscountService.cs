using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using System.Collections.Immutable;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Services;

/// <summary>
/// Service for managing discount application on sales.
/// Handles both manual and automatic discounts, ensuring business rules are enforced.
/// </summary>
public interface ISaleDiscountService
{
    /// <summary>
    /// Applies the specified discounts to a sale and triggers all automatic discounts.
    /// Validates that all discount IDs exist and that sales with discounts have at least 4 items.
    /// </summary>
    /// <param name="sale">The sale to apply discounts to.</param>
    /// <param name="discountIds">The collection of discount IDs to apply. If empty, only automatic discounts are applied.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when one or more discount IDs do not exist, or when discounts are requested for sales with fewer than 4 items.
    /// </exception>
    Task ApplyDiscounts(Sale sale, IImmutableList<Guid> discountIds, CancellationToken cancellationToken);
}