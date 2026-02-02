namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetUserSales;

public sealed record  GetUserSalesResponseItemDiscount
{
    public required string DiscountCode { get; init; }
    public decimal? Multiplier { get; set; }
    public decimal? Amount { get; set; }
}