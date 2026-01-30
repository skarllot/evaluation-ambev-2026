namespace Ambev.DeveloperEvaluation.Domain.Sales.Events;

public sealed record SaleCancelled : MediatR.INotification
{
    public required Guid SaleId { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
}