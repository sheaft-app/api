using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryReturnableConfiguration : IEntityTypeConfiguration<DeliveryReturnable>
    {
        public void Configure(EntityTypeBuilder<DeliveryReturnable> entity)
        {
            entity.Property(c => c.Quantity);
            entity.Property(c => c.RowVersion).IsRowVersion();

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            
            entity.Ignore(o => o.UnitOnSalePrice);
            entity.Ignore(o => o.UnitVatPrice);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("DeliveryReturnables");
        }
    }
}