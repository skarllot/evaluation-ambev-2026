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
        builder.Property(d => d.IsAutomatic).HasDefaultValue(false);
        builder.Property(d => d.Multiplier).HasPrecision(4, 4);
        builder.Property(d => d.Amount).HasPrecision(7, 2);

        builder.HasKey(d => d.Id);
        builder.HasIndex(d => d.Code).IsUnique();

        builder.HasData(
            new Discount
            {
                Id = new Guid("019c0efb-9347-75fa-8bf0-7211eebad895"),
                Code = DiscountCode.Auto4Items,
                IsAutomatic = true,
                Multiplier = 0.1m,
                Amount = null,
            },
            new Discount
            {
                Id = new Guid("019c0efb-9967-7c5b-9342-bdfb64a9e3e7"),
                Code = DiscountCode.Auto10Items,
                IsAutomatic = true,
                Multiplier = 0.2m,
                Amount = null,
            }
        );
    }
}
