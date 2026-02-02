using System.Collections.Immutable;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public sealed record CreateSaleRequest
{
    public required Guid CustomerId { get; init; }
    public required Guid BranchId { get; init; }
    public required ImmutableList<CreateSaleItemDto> Items { get; init; }
    public ImmutableList<Guid> Discounts { get; init; } = [];
}