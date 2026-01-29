using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

public class Sale : BaseEntity
{
    public required long Number { get; set; }

    public required Guid CustomerId { get; init; }
    public required Guid BranchId { get; init; }

    public List<SaleItem> Items { get; init; } = [];
    public List<Discount> Discounts { get; init; } = [];

    public bool IsCancelled { get; set; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
}
