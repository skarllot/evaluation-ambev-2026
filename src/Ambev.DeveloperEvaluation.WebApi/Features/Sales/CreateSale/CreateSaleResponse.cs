namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public sealed record CreateSaleResponse
{
    public required Guid Id { get; init; }
}