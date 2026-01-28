using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

public class SaleItem : BaseEntity
{
    public required Guid ProductId { get; init; }
    public required ProductQuantity Quantity { get; set; }
    public required ProductPrice UnitPrice { get; set; }
}