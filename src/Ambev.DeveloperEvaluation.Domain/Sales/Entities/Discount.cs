using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

public class Discount : BaseEntity
{
    public required DiscountCode Code { get; init; }
    public decimal? Multiplier { get; set; }
    public decimal? Amount { get; set; }
}
