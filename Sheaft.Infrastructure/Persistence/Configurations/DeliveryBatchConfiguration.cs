using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryBatchConfiguration : IEntityTypeConfiguration<DeliveryBatch>
    {
        private readonly bool _isAdmin;

        public DeliveryBatchConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<DeliveryBatch> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasMany(c => c.Deliveries)
                .WithOne(c => c.DeliveryBatch)
                .HasForeignKey(c => c.DeliveryBatchId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.AssignedTo)
                .WithMany()
                .HasForeignKey(c => c.AssignedToId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.ToTable("DeliveryBatches");
        }
    }
}