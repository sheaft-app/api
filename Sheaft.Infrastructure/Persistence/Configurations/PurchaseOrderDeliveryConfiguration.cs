using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderDeliveryConfiguration : IEntityTypeConfiguration<PurchaseOrderDelivery>
    {
        public PurchaseOrderDeliveryConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<PurchaseOrderDelivery> entity)
        {
            entity.OwnsOne(o => o.Address);
            entity.HasOne(c => c.DeliveryMode).WithMany().HasForeignKey(c => c.DeliveryModeId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("PurchaseOrderDeliveries");
        }
    }
}