using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property<long>("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property<long?>("ReturnableId");

        builder.HasKey("Id");
        
        builder
            .Property<DateTimeOffset>("CreatedOn")
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAdd();
        
        builder
            .Property<DateTimeOffset>("UpdatedOn")
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAddOrUpdate();

        builder.HasOne(c => c.Returnable)
            .WithMany()
            .HasForeignKey("ReturnableId");

        builder
            .Property(p => p.Name)
            .HasConversion(name => name.Value, value => new ProductName(value));

        builder
            .Property(p => p.Reference)
            .HasConversion(code => code.Value, value => new ProductReference(value));

        builder
            .Property(p => p.Vat)
            .HasConversion(vat => vat.Value, value => new VatRate(value));

        builder
            .Property(p => p.SupplierIdentifier)
            .HasConversion(vat => vat.Value, value => new SupplierId(value));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder
            .HasIndex(c => new {c.SupplierIdentifier, c.Reference})
            .IsUnique();
        
        builder.ToTable("Product");
    }
}