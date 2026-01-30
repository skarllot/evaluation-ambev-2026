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
            Items = command.Items.Select(ToSaleItemEntity).ToList(),
            IsCancelled = false,
            CreatedAt = timestamp.UtcDateTime,
        };
    }

    private static SaleItem ToSaleItemEntity(this CreateSaleItem command)
    {
        return new SaleItem
        {
            ProductId = command.ProductId,
            Quantity = new ProductQuantity(command.Quantity),
            UnitPrice = new ProductPrice(command.UnitPrice),
        };
    }
}
