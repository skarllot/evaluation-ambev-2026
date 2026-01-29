namespace Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;

public sealed record DiscountCode(string Value)
{
    public static DiscountCode Auto4Items { get; } = new("AUTO_10_PERCENT");
    public static DiscountCode Auto10Items { get; } = new("AUTO_20_PERCENT");
}
