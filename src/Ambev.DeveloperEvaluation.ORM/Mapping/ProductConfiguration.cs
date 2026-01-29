using Ambev.DeveloperEvaluation.Domain.Products.Entities;
using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
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
        builder.Property(p => p.UnitPrice).HasPrecision(8, 2).HasConversion(p => p.Amount, a => new ProductPrice(a));
        
        builder.HasKey(p => p.Id);
    }
}
