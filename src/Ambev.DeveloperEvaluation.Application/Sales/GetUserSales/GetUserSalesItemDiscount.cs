namespace Ambev.DeveloperEvaluation.Application.Sales.GetUserSales;

public sealed record GetUserSalesItemDiscount
{
    public required string DiscountCode { get; init; }
    public decimal? Multiplier { get; set; }
    public decimal? Amount { get; set; }
}