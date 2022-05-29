using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class CatalogProductConfiguration : IEntityTypeConfiguration<CatalogProduct>
{
    public void Configure(EntityTypeBuilder<CatalogProduct> builder)
    {
        builder.Property<ProductId>("ProductId");
        
        builder.HasKey("CatalogId", "ProductId");
        
        builder
            .Property(p => p.UnitPrice)
            .HasPrecision(18, 2)
            .HasConversion(price => price.Value, value => new ProductUnitPrice(value));

        builder.HasOne(c => c.Product)
            .WithMany()
            .HasForeignKey("ProductId")
            .OnDelete(DeleteBehavior.NoAction);;
        
        builder.ToTable("Catalog_Products");
    }
}