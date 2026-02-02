using System.Collections.Immutable;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public sealed record GetUserSalesResult
{
    public required int TotalCount { get; init; }
    public required ImmutableList<GetUserSalesItem> Items { get; init; }
}