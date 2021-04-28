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
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.OwnsOne(c => c.Address);
            entity.HasMany(c => c.DeliveryHours)
                .WithOne()
                .HasForeignKey(c => c.DeliveryModeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            entity.HasMany(c => c.Closings)
                .WithOne()
                .HasForeignKey(c => c.DeliveryModeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasOne(c => c.Producer)
                .WithMany()
                .HasForeignKey(c => c.ProducerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Ignore(c => c.DomainEvents);
            
            var deliveryHours = entity.Metadata.FindNavigation(nameof(DeliveryMode.DeliveryHours));
            deliveryHours.SetPropertyAccessMode(PropertyAccessMode.Field);

            var closings = entity.Metadata.FindNavigation(nameof(DeliveryMode.Closings));
            closings.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey(c => c.Id);
            entity.ToTable("DeliveryModes");
        }
    }
}
