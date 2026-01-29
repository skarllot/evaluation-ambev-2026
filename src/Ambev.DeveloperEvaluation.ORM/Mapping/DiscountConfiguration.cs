using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Ambev.DeveloperEvaluation.ORM.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discounts");

        builder.Property(d => d.Id).HasValueGenerator<NpgsqlSequentialGuidValueGenerator>();
        builder.Property(d => d.Code).HasMaxLength(20).HasConversion(d => d.Value, v => new DiscountCode(v));
        builder.Property(d => d.Multiplier).HasPrecision(4, 4);
        builder.Property(d => d.Amount).HasPrecision(7, 2);
    }
}
