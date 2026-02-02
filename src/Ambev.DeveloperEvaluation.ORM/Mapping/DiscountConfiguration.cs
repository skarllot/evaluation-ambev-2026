using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
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
        builder.OwnsOne(
            d => d.Code,
            b =>
            {
                b.Property(x => x.Value).HasMaxLength(20).HasColumnName("Code");
                b.HasIndex(x => x.Value).IsUnique();
            }
        );
        builder.Property(d => d.IsAutomatic).HasDefaultValue(false);
        builder.Property(d => d.Multiplier).HasPrecision(4, 4);
        builder.Property(d => d.Amount).HasPrecision(7, 2);

        builder.HasKey(d => d.Id);
    }
}
