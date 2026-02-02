using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale.TestData;

public static class CreateSaleCommandTestData
{
    public static CreateSaleCommand GenerateValidCommand()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new CreateSaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5,
                    UnitPrice = 10.50m,
                },
                new CreateSaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 3,
                    UnitPrice = 25.00m,
                },
            ],
            Discounts = [Guid.NewGuid()],
        };
    }

    public static CreateSaleCommand GenerateCommandWithEmptyCustomerId()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.NewGuid(),
            Items =
            [
                new CreateSaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5,
                    UnitPrice = 10.50m,
                },
            ],
        };
    }

    public static CreateSaleCommand GenerateCommandWithEmptyBranchId()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.Empty,
            Items =
            [
                new CreateSaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5,
                    UnitPrice = 10.50m,
                },
            ],
        };
    }

    public static CreateSaleCommand GenerateCommandWithInvalidItem(
        Guid? productId = null,
        int? quantity = null,
        decimal? unitPrice = null
    )
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new CreateSaleItem
                {
                    ProductId = productId ?? Guid.NewGuid(),
                    Quantity = quantity ?? 5,
                    UnitPrice = unitPrice ?? 10.50m,
                },
            ],
        };
    }

    public static CreateSaleCommand GenerateCommandWithEmptyDiscount()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new CreateSaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5,
                    UnitPrice = 10.50m,
                },
            ],
            Discounts = [Guid.Empty],
        };
    }
}
