namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public sealed record CreateSaleItem
{
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
}