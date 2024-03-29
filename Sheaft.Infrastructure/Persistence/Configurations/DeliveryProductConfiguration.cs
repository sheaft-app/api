using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryProductConfiguration : IEntityTypeConfiguration<DeliveryProduct>
    {
        public void Configure(EntityTypeBuilder<DeliveryProduct> entity)
        {
            entity.Property(c => c.Quantity);
            entity.Property(c => c.RowVersion).IsRowVersion();

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitWeight).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalWeight).HasColumnType("decimal(10,2)");

            entity.Property(o => o.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            entity.Property(o => o.Reference).IsRequired();

            entity.Property(o => o.ReturnableVat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.ReturnableWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalReturnableOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalProductOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Ignore(o => o.UnitOnSalePrice);
            entity.Ignore(o => o.UnitVatPrice);
            entity.Ignore(o => o.ReturnableVatPrice);
            entity.Ignore(o => o.ReturnableOnSalePrice);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("DeliveryProducts");
        }
    }
}