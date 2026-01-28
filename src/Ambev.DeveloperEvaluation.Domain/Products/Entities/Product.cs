using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Products.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public required ProductPrice UnitPrice { get; set; }
}
