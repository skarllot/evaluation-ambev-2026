namespace Ambev.DeveloperEvaluation.Domain.Sales.Events;

public sealed record ItemCancelled : MediatR.INotification
{
    public required Guid SaleId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
}