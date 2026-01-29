using System.Collections.Immutable;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public sealed record CreateSaleCommand : IRequest<CreateSaleResult>
{
    public required Guid CustomerId { get; init; }
    public required Guid BranchId { get; init; }
    public required ImmutableList<CreateSaleItem> Items { get; init; }
    public ImmutableList<Guid> Discounts { get; init; } = [];
}