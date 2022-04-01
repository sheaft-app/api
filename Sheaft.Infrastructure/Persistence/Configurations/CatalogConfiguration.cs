using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder
            .Property<long>("Id")
            .ValueGeneratedOnAdd();

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

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey("CatalogId");
        
        builder
            .Navigation(c => c.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_products");

        builder
            .Property(p => p.Name)
            .HasConversion(name => name.Value, value => new CatalogName(value));

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Catalog");
    }
}