namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public interface IGetUserSalesQuery
{
    Task<int> Count(
        Guid customerId,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        CancellationToken cancellationToken
    );

    Task<List<GetUserSalesItem>> Get(
        Guid customerId,
        int pageNumber,
        int pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        CancellationToken cancellationToken
    );
}
