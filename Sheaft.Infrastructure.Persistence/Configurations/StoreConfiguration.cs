using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> entity)
        {
            entity.OwnsMany(c => c.OpeningHours, cb =>
            {
                cb.ToTable("StoreOpeningHours");
            });

            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("StoreUid").OnDelete(DeleteBehavior.Cascade);

            var businessTags = entity.Metadata.FindNavigation(nameof(Store.Tags));
            businessTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            var businessHours = entity.Metadata.FindNavigation(nameof(Store.OpeningHours));
            businessHours.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
