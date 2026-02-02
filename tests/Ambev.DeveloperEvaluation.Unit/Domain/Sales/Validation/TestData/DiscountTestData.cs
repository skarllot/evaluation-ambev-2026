using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Validation.TestData;

public static class DiscountTestData
{
    public static Discount GenerateValidDiscountWithMultiplier()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = Math.Round(faker.Random.Decimal(0.0001m, 0.9999m), 4),
            Amount = null,
        };
    }

    public static Discount GenerateValidDiscountWithAmount()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = null,
            Amount = Math.Round(faker.Random.Decimal(0.01m, 99999.99m), 2),
        };
    }

    public static Discount GenerateInvalidDiscountWithBothValues()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = Math.Round(faker.Random.Decimal(0.0001m, 0.9999m), 4),
            Amount = Math.Round(faker.Random.Decimal(0.01m, 99999.99m), 2),
        };
    }

    public static Discount GenerateInvalidDiscountWithNeitherValue()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = null,
            Amount = null,
        };
    }

    public static Discount GenerateDiscountWithEmptyCode()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(""),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = faker.Random.Decimal(0.0001m, 0.9999m),
            Amount = null,
        };
    }

    public static Discount GenerateDiscountWithInvalidMultiplier(decimal multiplier)
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = multiplier,
            Amount = null,
        };
    }

    public static Discount GenerateDiscountWithInvalidAmount(decimal amount)
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = null,
            Amount = amount,
        };
    }

    public static Discount GenerateDiscountWithHighPrecisionMultiplier()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = 0.12345m, // 5 decimal places
            Amount = null,
        };
    }

    public static Discount GenerateDiscountWithHighPrecisionAmount()
    {
        var faker = new Faker();
        return new Discount
        {
            Code = new DiscountCode(faker.Random.String2(5, 20).ToUpper()),
            IsAutomatic = faker.Random.Bool(),
            Multiplier = null,
            Amount = 10.123m, // 3 decimal places
        };
    }
}
