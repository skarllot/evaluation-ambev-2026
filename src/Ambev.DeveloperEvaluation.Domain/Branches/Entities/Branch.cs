using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Branches.Entities;

public class Branch : BaseEntity
{
    public required string Name { get; set; }
}