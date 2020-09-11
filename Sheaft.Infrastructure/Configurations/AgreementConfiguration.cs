using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Sheaft.Infrastructure
{
    public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
    {
        public void Configure(EntityTypeBuilder<Agreement> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("DeliveryModeUid");
            entity.Property<long>("StoreUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.OwnsMany(c => c.SelectedHours, cb =>
            {
                cb.ToTable("AgreementSelectedHours");
            });

            entity.HasOne(c => c.Delivery).WithMany().HasForeignKey("DeliveryModeUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Store).WithMany().HasForeignKey("StoreUid").OnDelete(DeleteBehavior.Cascade);

            var hours = entity.Metadata.FindNavigation(nameof(Agreement.SelectedHours));
            hours.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("StoreUid");
            entity.HasIndex("DeliveryModeUid");
            entity.HasIndex("Uid", "Id", "StoreUid", "DeliveryModeUid", "RemovedOn");

            entity.ToTable("Agreements");
        }
    }
}
