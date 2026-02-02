using System.Collections.Immutable;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetUserSales;

public sealed record GetUserSalesResponseItem
{
    public required long SaleNumber { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required decimal TotalAmount { get; init; }
    public required Guid BranchId { get; init; }
    public required string BranchName { get; init; }
    public required ImmutableList<GetUserSalesResponseItemProduct> Products { get; init; }
    public required ImmutableList<GetUserSalesResponseItemDiscount> Discounts { get; init; }
    public required bool IsCancelled { get; init; }
}