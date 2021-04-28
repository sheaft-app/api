using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CatalogProductConfiguration : IEntityTypeConfiguration<CatalogProduct>
    {
        public void Configure(EntityTypeBuilder<CatalogProduct> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            entity.Property(o => o.VatPricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.VatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePrice).HasColumnType("decimal(10,2)");
            
            entity.ToTable("CatalogProducts");
        }
    }
}