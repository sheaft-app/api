using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class OrderDeliveryConfiguration : IEntityTypeConfiguration<OrderDelivery>
    {
        public void Configure(EntityTypeBuilder<OrderDelivery> entity)
        {
            entity.Property<long>("OrderUid");

            entity.HasKey("OrderUid", "Id");

            entity.OwnsOne(c => c.ExpectedDelivery);
            entity.HasOne(c => c.DeliveryMode).WithMany().HasForeignKey("DeliveryModeUid");

            entity.ToTable("OrderDeliveries");
        }
    }
}
