using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation.TestData;

public static class SaleItemTestData
{
    public static SaleItem GenerateValidSaleItem()
    {
        var faker = new Faker();
        return new SaleItem
        {
            ProductId = Guid.NewGuid(),
            Quantity = new ProductQuantity(faker.Random.Int(1, 20)),
            UnitPrice = new ProductPrice(Math.Round(faker.Random.Decimal(0.01m, 999999.99m), 2)),
        };
    }

    public static SaleItem GenerateSaleItemWithEmptyProductId()
    {
        var faker = new Faker();
        return new SaleItem
        {
            ProductId = Guid.Empty,
            Quantity = new ProductQuantity(faker.Random.Int(1, 20)),
            UnitPrice = new ProductPrice(Math.Round(faker.Random.Decimal(0.01m, 999999.99m), 2)),
        };
    }

    public static SaleItem GenerateSaleItemWithInvalidQuantity(int quantity)
    {
        var faker = new Faker();
        return new SaleItem
        {
            ProductId = Guid.NewGuid(),
            Quantity = new ProductQuantity(quantity),
            UnitPrice = new ProductPrice(Math.Round(faker.Random.Decimal(0.01m, 999999.99m), 2)),
        };
    }

    public static SaleItem GenerateSaleItemWithInvalidPrice(decimal price)
    {
        var faker = new Faker();
        return new SaleItem
        {
            ProductId = Guid.NewGuid(),
            Quantity = new ProductQuantity(faker.Random.Int(1, 20)),
            UnitPrice = new ProductPrice(price),
        };
    }

    public static SaleItem GenerateSaleItemWithHighPrecisionPrice()
    {
        var faker = new Faker();
        return new SaleItem
        {
            ProductId = Guid.NewGuid(),
            Quantity = new ProductQuantity(faker.Random.Int(1, 20)),
            UnitPrice = new ProductPrice(10.123m), // 3 decimal places
        };
    }
}
