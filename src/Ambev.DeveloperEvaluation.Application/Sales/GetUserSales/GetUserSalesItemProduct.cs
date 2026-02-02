namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public sealed record GetUserSalesItemProduct
{
    public required Guid ProductId { get; init; }
    public required string ProductName { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
    public required decimal TotalAmount { get; init; }
}