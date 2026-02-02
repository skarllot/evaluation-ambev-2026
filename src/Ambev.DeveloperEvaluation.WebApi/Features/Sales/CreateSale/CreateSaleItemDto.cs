namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public sealed record CreateSaleItemDto
{
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
}