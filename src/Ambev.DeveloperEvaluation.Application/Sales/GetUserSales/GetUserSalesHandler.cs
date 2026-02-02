using System.Collections.Immutable;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public class GetUserSalesHandler(IGetUserSalesQuery query) : IRequestHandler<GetUserSalesCommand, GetUserSalesResult>
{
    public async Task<GetUserSalesResult> Handle(GetUserSalesCommand request, CancellationToken cancellationToken)
    {
        request.ValidateAndThrow();

        return new GetUserSalesResult
        {
            TotalCount = await query.Count(
                customerId: request.CustomerId,
                startDate: request.StartDate,
                endDate: request.EndDate,
                cancellationToken: cancellationToken
            ),
            Items = (
                await query.Get(
                    customerId: request.CustomerId,
                    pageNumber: request.PageNumber,
                    pageSize: request.PageSize,
                    startDate: request.StartDate,
                    endDate: request.EndDate,
                    cancellationToken: cancellationToken
                )
            ).ToImmutableList(),
        };
    }
}
