using Ambev.DeveloperEvaluation.Domain.Products.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
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
        builder.ComplexProperty(si => si.Quantity, b => b.Property(x => x.Value).HasColumnName("Quantity"));
        builder.ComplexProperty(
            si => si.UnitPrice,
            b => b.Property(x => x.Amount).HasPrecision(8, 2).HasColumnName("UnitPrice")
        );

        builder.HasKey(si => si.Id);

        builder.HasOne<Product>().WithMany().HasForeignKey(si => si.ProductId);
    }
}
