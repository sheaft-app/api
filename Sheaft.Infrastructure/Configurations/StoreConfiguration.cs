using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> entity)
        {
            entity.Property(c => c.Siret).IsRequired();

            entity.OwnsOne(c => c.LegalAddress, cb =>
            {
                cb.ToTable("StoreLegalAddresses");
            });

            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("StoreUid").OnDelete(DeleteBehavior.Cascade);

            var companyTags = entity.Metadata.FindNavigation(nameof(Store.Tags));
            companyTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.OwnsMany(c => c.OpeningHours, cb =>
            {
                cb.ToTable("StoreOpeningHours");
            });

            var companyHours = entity.Metadata.FindNavigation(nameof(Store.OpeningHours));
            companyHours.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
