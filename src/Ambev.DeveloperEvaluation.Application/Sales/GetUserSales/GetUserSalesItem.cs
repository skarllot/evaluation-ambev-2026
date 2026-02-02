using System.Collections.Immutable;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public sealed record GetUserSalesItem
{
    public required long SaleNumber { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required decimal TotalAmount { get; init; }
    public required Guid BranchId { get; init; }
    public required string BranchName { get; init; }
    public required ImmutableList<GetUserSalesItemProduct> Products { get; init; }
    public required ImmutableList<GetUserSalesItemDiscount> Discounts { get; init; }
    public required bool IsCancelled { get; init; }
}