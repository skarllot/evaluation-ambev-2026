using Ambev.DeveloperEvaluation.Domain.Products.Entities;
using Ambev.DeveloperEvaluation.ORM.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(p => p.Id).HasValueGenerator<NpgsqlSequentialGuidValueGenerator>();
        builder.Property(p => p.Name).HasMaxLength(128);
        builder.ComplexProperty(
            p => p.UnitPrice,
            b => b.Property(p => p.Amount).HasPrecision(8, 2).HasColumnName("UnitPrice")
        );

        builder.HasKey(p => p.Id);
    }
}
