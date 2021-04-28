using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class OrderDeliveryConfiguration : IEntityTypeConfiguration<OrderDelivery>
    {
        public void Configure(EntityTypeBuilder<OrderDelivery> entity)
        {
            entity.HasKey(c => new {c.OrderId, c.DeliveryModeId});

            entity.OwnsOne(c => c.ExpectedDelivery);
            entity.HasOne(c => c.DeliveryMode).WithMany().HasForeignKey(c =>c.DeliveryModeId).OnDelete(DeleteBehavior.NoAction);

            entity.ToTable("OrderDeliveries");
        }
    }
}
