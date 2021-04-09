using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
    {
        public void Configure(EntityTypeBuilder<Agreement> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("DeliveryModeUid");
            entity.Property<long>("StoreUid");
            entity.Property<long?>("CatalogUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.OwnsMany(c => c.SelectedHours, cb =>
            {
                cb.ToTable("AgreementSelectedHours");
            });

            entity.Ignore(c => c.DomainEvents);

            entity.HasOne(c => c.Delivery).WithMany().HasForeignKey("DeliveryModeUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Store).WithMany().HasForeignKey("StoreUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Catalog).WithMany().HasForeignKey("CatalogUid").OnDelete(DeleteBehavior.NoAction);

            var hours = entity.Metadata.FindNavigation(nameof(Agreement.SelectedHours));
            hours.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("StoreUid");
            entity.HasIndex("DeliveryModeUid");
            entity.HasIndex("CatalogUid");
            entity.HasIndex("Uid", "Id", "StoreUid", "DeliveryModeUid", "CatalogUid", "RemovedOn");

            entity.ToTable("Agreements");
        }
    }
}
