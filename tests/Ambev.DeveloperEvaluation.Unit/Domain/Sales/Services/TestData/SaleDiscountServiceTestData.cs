using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Sales.Services.TestData;

/// <summary>
/// Provides methods for generating test data for SaleDiscountService tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleDiscountServiceTestData
{
    /// <summary>
    /// Gets a Faker instance configured to generate valid Sale entities.
    /// Returns a new instance each time to allow chaining additional rules.
    /// </summary>
    private static Faker<Sale> SaleFaker =>
        new Faker<Sale>()
            .RuleFor(s => s.Id, _ => Guid.NewGuid())
            .RuleFor(s => s.Number, f => f.Random.Long(1, 999999))
            .RuleFor(s => s.CustomerId, _ => Guid.NewGuid())
            .RuleFor(s => s.BranchId, _ => Guid.NewGuid())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past())
            .RuleFor(s => s.Items, _ => []);

    /// <summary>
    /// Gets a Faker instance configured to generate valid SaleItem entities.
    /// Returns a new instance each time to allow chaining additional rules.
    /// </summary>
    private static Faker<SaleItem> SaleItemFaker =>
        new Faker<SaleItem>()
            .RuleFor(si => si.Id, _ => Guid.NewGuid())
            .RuleFor(si => si.ProductId, _ => Guid.NewGuid())
            .RuleFor(si => si.Quantity, f => new ProductQuantity(f.Random.Int(1, 20)))
            .RuleFor(si => si.UnitPrice, f => new ProductPrice(f.Random.Decimal(1, 100)));

    /// <summary>
    /// Gets a Faker instance configured to generate valid Discount entities.
    /// Returns a new instance each time to allow chaining additional rules.
    /// </summary>
    private static Faker<Discount> DiscountFaker =>
        new Faker<Discount>()
            .RuleFor(d => d.Id, _ => Guid.NewGuid())
            .RuleFor(d => d.Code, f => new DiscountCode($"DISC{f.Random.Number(10, 99)}"))
            .RuleFor(d => d.IsAutomatic, _ => false)
            .RuleFor(d => d.Multiplier, f => f.Random.Decimal(0.05m, 0.30m))
            .RuleFor(d => d.Amount, _ => null);

    /// <summary>
    /// Generates a valid Sale entity with specified total quantity.
    /// </summary>
    /// <param name="totalQuantity">Total quantity of items in the sale.</param>
    /// <returns>A valid Sale entity with the specified quantity.</returns>
    public static Sale GenerateSale(int totalQuantity)
    {
        return SaleFaker.RuleFor(s => s.Items, _ => [GenerateSaleItem(totalQuantity)]).Generate();
    }

    /// <summary>
    /// Generates a valid Sale entity with multiple items totaling the specified quantity.
    /// </summary>
    /// <param name="totalQuantity">Total quantity of items across all sale items.</param>
    /// <param name="itemCount">Number of distinct items in the sale.</param>
    /// <returns>A valid Sale entity with multiple items.</returns>
    public static Sale GenerateSaleWithMultipleItems(int totalQuantity, int itemCount)
    {
        var quantityPerItem = totalQuantity / itemCount;
        var remainder = totalQuantity % itemCount;

        return SaleFaker
            .RuleFor(
                s => s.Items,
                _ =>
                {
                    var items = new List<SaleItem>();
                    for (int i = 0; i < itemCount; i++)
                    {
                        var quantity = quantityPerItem + (i == 0 ? remainder : 0);
                        items.Add(GenerateSaleItem(quantity));
                    }
                    return items;
                }
            )
            .Generate();
    }

    /// <summary>
    /// Generates a valid SaleItem entity with specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity for the sale item.</param>
    /// <returns>A valid SaleItem entity.</returns>
    public static SaleItem GenerateSaleItem(int quantity)
    {
        return SaleItemFaker.RuleFor(si => si.Quantity, _ => new ProductQuantity(quantity)).Generate();
    }

    /// <summary>
    /// Generates a valid Discount entity.
    /// </summary>
    /// <param name="id">The discount ID. If not provided, a new GUID is generated.</param>
    /// <param name="code">The discount code. If not provided, a random code is generated.</param>
    /// <param name="isAutomatic">Whether the discount is automatic.</param>
    /// <returns>A valid Discount entity.</returns>
    public static Discount GenerateDiscount(Guid? id = null, string? code = null, bool isAutomatic = false)
    {
        return DiscountFaker
            .RuleFor(d => d.Id, _ => id ?? Guid.NewGuid())
            .RuleFor(d => d.Code, f => new DiscountCode(code ?? $"DISC{f.Random.Number(10, 99)}"))
            .RuleFor(d => d.IsAutomatic, _ => isAutomatic)
            .Generate();
    }

    /// <summary>
    /// Generates multiple valid Discount entities.
    /// </summary>
    /// <param name="count">The number of discounts to generate.</param>
    /// <param name="isAutomatic">Whether the discounts are automatic.</param>
    /// <returns>A list of valid Discount entities.</returns>
    public static List<Discount> GenerateDiscounts(int count, bool isAutomatic = false)
    {
        return DiscountFaker.RuleFor(d => d.IsAutomatic, _ => isAutomatic).Generate(count);
    }
}
