using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CatalogProductConfiguration : IEntityTypeConfiguration<CatalogProduct>
    {
        public void Configure(EntityTypeBuilder<CatalogProduct> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("CatalogUid");
            entity.Property<long>("ProductUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            entity.Property(o => o.VatPricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.VatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePrice).HasColumnType("decimal(10,2)");
            
            entity.HasKey("Uid");
            entity.HasIndex("CatalogUid", "ProductUid").IsUnique();
            entity.ToTable("CatalogProducts");
        }
    }
}