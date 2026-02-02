using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public static class CreateSaleMapper
{
    public static Sale ToSaleEntity(this CreateSaleCommand command, DateTimeOffset timestamp)
    {
        return new Sale
        {
            CustomerId = command.CustomerId,
            BranchId = command.BranchId,
            Items = command.Items.GroupBy(i => i.ProductId).Select(ToSaleItemEntity).ToList(),
            IsCancelled = false,
            CreatedAt = timestamp.UtcDateTime,
        };
    }

    private static SaleItem ToSaleItemEntity(this IGrouping<Guid, CreateSaleItem> items)
    {
        return new SaleItem
        {
            ProductId = items.Key,
            Quantity = new ProductQuantity(items.Sum(i => i.Quantity)),
            UnitPrice = new ProductPrice(items.Sum(i => i.UnitPrice)),
        };
    }
}
