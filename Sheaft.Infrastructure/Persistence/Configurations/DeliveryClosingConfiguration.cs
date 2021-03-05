using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryClosingConfiguration : IEntityTypeConfiguration<DeliveryClosing>
    {
        public void Configure(EntityTypeBuilder<DeliveryClosing> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("DeliveryModeUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("DeliveryModeUid");
            entity.HasIndex("Uid", "Id", "DeliveryModeUid", "RemovedOn");

            entity.ToTable("DeliveryClosings");
        }
    }
}