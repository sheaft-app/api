using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class DeliveryModeConfiguration : IEntityTypeConfiguration<DeliveryMode>
    {
        private readonly bool _isAdmin;

        public DeliveryModeConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }
        
        public void Configure(EntityTypeBuilder<DeliveryMode> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ProducerUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.OwnsOne(c => c.Address);
            entity.OwnsMany(c => c.OpeningHours, cb =>
            {
                cb.ToTable("DeliveryModeOpeningHours");
            });
            
            entity.HasMany(c => c.Closings).WithOne().HasForeignKey("DeliveryModeUid").OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.Ignore(c => c.DomainEvents);
            
            var deliveryHours = entity.Metadata.FindNavigation(nameof(DeliveryMode.OpeningHours));
            deliveryHours.SetPropertyAccessMode(PropertyAccessMode.Field);

            var closings = entity.Metadata.FindNavigation(nameof(DeliveryMode.Closings));
            closings.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("ProducerUid");
            entity.HasIndex("Uid", "Id", "ProducerUid", "RemovedOn");

            entity.ToTable("DeliveryModes");
        }
    }
}
