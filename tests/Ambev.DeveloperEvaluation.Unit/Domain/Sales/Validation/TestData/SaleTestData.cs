using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation.TestData;

public static class SaleTestData
{
    public static Sale GenerateValidSale()
    {
        var faker = new Faker();
        return new Sale
        {
            Number = faker.Random.Long(1, 999999),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Items =
            [
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(3),
                    UnitPrice = new ProductPrice(25.00m),
                },
            ],
        };
    }

    public static Sale GenerateSaleWithInvalidNumber(long number)
    {
        return new Sale
        {
            Number = number,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Items =
            [
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
            ],
        };
    }

    public static Sale GenerateSaleWithEmptyCustomerId()
    {
        var faker = new Faker();
        return new Sale
        {
            Number = faker.Random.Long(1, 999999),
            CustomerId = Guid.Empty,
            BranchId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Items =
            [
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
            ],
        };
    }

    public static Sale GenerateSaleWithEmptyBranchId()
    {
        var faker = new Faker();
        return new Sale
        {
            Number = faker.Random.Long(1, 999999),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.Empty,
            CreatedAt = DateTime.UtcNow,
            Items =
            [
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
            ],
        };
    }

    public static Sale GenerateSaleWithEmptyCreatedAt()
    {
        var faker = new Faker();
        return new Sale
        {
            Number = faker.Random.Long(1, 999999),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            CreatedAt = default,
            Items =
            [
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
            ],
        };
    }

    public static Sale GenerateSaleWithDuplicateProducts()
    {
        var faker = new Faker();
        var productId = Guid.NewGuid();
        return new Sale
        {
            Number = faker.Random.Long(1, 999999),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Items =
            [
                new SaleItem
                {
                    ProductId = productId,
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
                new SaleItem
                {
                    ProductId = productId,
                    Quantity = new ProductQuantity(3),
                    UnitPrice = new ProductPrice(10.50m),
                },
            ],
        };
    }

    public static Sale GenerateSaleWithDuplicateDiscounts()
    {
        var faker = new Faker();
        var discount = new Discount
        {
            Code = new DiscountCode("PROMO10"),
            IsAutomatic = false,
            Multiplier = 0.1m,
            Amount = null,
        };

        return new Sale
        {
            Number = faker.Random.Long(1, 999999),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Items =
            [
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = new ProductQuantity(5),
                    UnitPrice = new ProductPrice(10.50m),
                },
            ],
            Discounts = [discount, discount],
        };
    }
}
