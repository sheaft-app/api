using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public DeliveryConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Delivery> entity)
        {
            entity.Property(c => c.Client).UseCollation("Latin1_general_CI_AI");
            
            entity.OwnsOne(o => o.Address);
            
            entity.HasMany(c => c.PurchaseOrders).WithOne()
                .HasForeignKey(c => c.DeliveryId).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne<Producer>().WithMany()
                .HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne<User>().WithMany()
                .HasForeignKey(c => c.ClientId).OnDelete(DeleteBehavior.NoAction);

            entity.HasKey(c => c.Id);
            entity.ToTable("Deliveries");
        }
    }
}