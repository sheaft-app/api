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
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();
            
            entity.Property(o => o.WholeSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePrice).HasColumnType("decimal(10,2)");
            
            entity.Ignore(o => o.VatPricePerUnit);
            entity.Ignore(o => o.OnSalePricePerUnit);
            entity.Ignore(o => o.VatPrice);
            entity.Ignore(o => o.OnSalePrice);
            
            entity.ToTable("CatalogProducts");
        }
    }
}