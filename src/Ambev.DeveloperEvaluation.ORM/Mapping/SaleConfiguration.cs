using Ambev.DeveloperEvaluation.Domain.Branches.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Users.Entities;
using Ambev.DeveloperEvaluation.ORM.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.Property(s => s.Id).HasValueGenerator<NpgsqlSequentialGuidValueGenerator>();
        builder.Property(s => s.Number).UseHiLo("SaleNumber");
        builder.Property(s => s.CustomerId);
        builder.Property(s => s.BranchId);
        builder.Property(s => s.IsCancelled);
        builder.Property(s => s.CreatedAt);
        builder.Property(s => s.UpdatedAt);
        
        builder.HasKey(s => s.Id);
        builder.HasAlternateKey(s => s.Number);

        builder.HasOne<User>().WithMany().HasForeignKey(s => s.CustomerId);
        builder.HasOne<Branch>().WithMany().HasForeignKey(s => s.BranchId);
        builder.HasMany(s => s.Items).WithOne().HasForeignKey("SaleId");
        builder.HasMany(s => s.Discounts).WithMany();
    }
}