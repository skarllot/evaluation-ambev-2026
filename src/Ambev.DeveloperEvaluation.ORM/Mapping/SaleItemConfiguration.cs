using Ambev.DeveloperEvaluation.Domain.Products.Entities;
using Ambev.DeveloperEvaluation.Domain.Products.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.ValueObjects;
using Ambev.DeveloperEvaluation.ORM.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.Property(si => si.Id).HasValueGenerator<NpgsqlSequentialGuidValueGenerator>();
        builder.Property(si => si.ProductId);
        builder.Property(si => si.Quantity).HasConversion(q => q.Value, v => new ProductQuantity(v));
        builder.Property(si => si.UnitPrice).HasPrecision(8, 2).HasConversion(p => p.Amount, v => new ProductPrice(v));
        
        builder.HasKey(si => si.Id);
        
        builder.HasOne<Product>().WithMany().HasForeignKey(si => si.ProductId);
    }
}