namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetUserSales;

public sealed record  GetUserSalesResponseItemProduct
{
    public required Guid ProductId { get; init; }
    public required string ProductName { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
    public required decimal TotalAmount { get; init; }
}