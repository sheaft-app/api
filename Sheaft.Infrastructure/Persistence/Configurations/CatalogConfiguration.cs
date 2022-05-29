using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey("CatalogId");
        
        builder
            .Navigation(c => c.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_products");

        builder
            .Property(p => p.Name)
            .HasMaxLength(Catalog.NAME_MAXLENGTH)
            .HasConversion(name => name.Value, value => new CatalogName(value));

        builder
            .Property(p => p.SupplierId)
            .HasMaxLength(Constants.IDS_LENGTH)
            .HasConversion(vat => vat.Value, value => new SupplierId(value));
        
        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey(c => c.CatalogId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("Catalog");
    }
}