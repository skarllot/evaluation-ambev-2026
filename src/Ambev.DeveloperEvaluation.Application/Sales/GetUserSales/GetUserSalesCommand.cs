using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public sealed record GetUserSalesCommand : IRequest<GetUserSalesResult>
{
    public required Guid CustomerId { get; init; }

    public DateTimeOffset? StartDate { get; init; }
    public DateTimeOffset? EndDate { get; init; }

    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}