using Ambev.DeveloperEvaluation.Domain.Branches.Entities;
using Ambev.DeveloperEvaluation.ORM.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class BranchConfiguration:IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.Property(b => b.Id).HasValueGenerator<NpgsqlSequentialGuidValueGenerator>();
        builder.Property(b => b.Name).HasMaxLength(128);
        
        builder.HasKey(b => b.Id);
    }
}