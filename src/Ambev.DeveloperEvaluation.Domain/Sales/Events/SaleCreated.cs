namespace Ambev.DeveloperEvaluation.Domain.Sales.Events;

public sealed record SaleCreated : MediatR.INotification
{
    public required Guid SaleId { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
}