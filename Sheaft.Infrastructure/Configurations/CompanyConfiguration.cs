using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> entity)
        {
            entity.Property<long>("Uid");
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Siret).IsRequired();
            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();
            entity.Property(c => c.Kind);

            entity.OwnsOne(c => c.Address, cb =>
            {
                cb.ToTable("CompanyAddresses");
            });

            entity.OwnsMany(c => c.OpeningHours, cb =>
            {
                cb.ToTable("CompanyOpeningHours");
            });

            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("CompanyUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<Agreement>().WithOne(c => c.Store).HasForeignKey("StoreUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<DeliveryMode>().WithOne(c => c.Producer).HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<Packaging>().WithOne(c => c.Producer).HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);

            var companyTags = entity.Metadata.FindNavigation(nameof(Company.Tags));
            companyTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            var companyHours = entity.Metadata.FindNavigation(nameof(Company.OpeningHours));
            companyHours.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("Uid", "Id", "CreatedOn");

            entity.ToTable("Companies");
        }
    }
}
