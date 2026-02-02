using System.Collections.Immutable;
using Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;
using Ambev.DeveloperEvaluation.Domain.Branches.Entities;
using Ambev.DeveloperEvaluation.Domain.Products.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Queries;

public class GetUserSalesQuery(DefaultContext dbContext) : IGetUserSalesQuery
{
    public static IQueryable<Sale> CountQuery(
        DefaultContext dbContext,
        Guid customerId,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate
    )
    {
        return dbContext
            .Set<Sale>()
            .Where(s =>
                s.CustomerId == customerId
                && (startDate == null || (s.UpdatedAt ?? s.CreatedAt) >= startDate)
                && (endDate == null || (s.UpdatedAt ?? s.CreatedAt) <= endDate)
            );
    }

    public static IQueryable<GetUserSalesItem> GetQuery(
        DefaultContext dbContext,
        Guid customerId,
        int pageNumber,
        int pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate
    )
    {
        return (
            from s in dbContext.Set<Sale>()
            join b in dbContext.Set<Branch>() on s.BranchId equals b.Id
            where
                s.CustomerId == customerId
                && (startDate == null || (s.UpdatedAt ?? s.CreatedAt) >= startDate)
                && (endDate == null || (s.UpdatedAt ?? s.CreatedAt) <= endDate)
            let discountMultiplier = s.Discounts.Sum(d => d.Multiplier) ?? 0m
            let discountAmount = s.Discounts.Sum(d => d.Amount) ?? 0m
            select new GetUserSalesItem
            {
                SaleNumber = s.Number,
                Date = s.UpdatedAt ?? s.CreatedAt,
                TotalAmount =
                    s.Items.Sum(i => i.Quantity.Value * i.UnitPrice.Amount) * (1m - discountMultiplier)
                    - discountAmount,
                BranchId = b.Id,
                BranchName = b.Name,
                Products = (
                    from i in s.Items
                    join p in dbContext.Set<Product>() on i.ProductId equals p.Id
                    select new GetUserSalesItemProduct
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Quantity = i.Quantity.Value,
                        UnitPrice = i.UnitPrice.Amount,
                        TotalAmount = i.Quantity.Value * i.UnitPrice.Amount,
                    }
                ).ToImmutableList(),
                Discounts = s
                    .Discounts.Select(d => new GetUserSalesItemDiscount
                    {
                        DiscountCode = d.Code.Value,
                        Multiplier = d.Multiplier,
                        Amount = d.Amount,
                    })
                    .ToImmutableList(),
                IsCancelled = s.IsCancelled,
            }
        )
            .OrderByDescending(x => x.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public Task<int> Count(
        Guid customerId,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        CancellationToken cancellationToken
    )
    {
        return CountQuery(dbContext, customerId, startDate, endDate).CountAsync(cancellationToken);
    }

    public Task<List<GetUserSalesItem>> Get(
        Guid customerId,
        int pageNumber,
        int pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        CancellationToken cancellationToken
    )
    {
        return GetQuery(dbContext, customerId, pageNumber, pageSize, startDate, endDate).ToListAsync(cancellationToken);
    }
}
