using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> entity)
        {
            entity.Property(c => c.Siret).IsRequired();

            entity.OwnsOne(c => c.LegalAddress, cb =>
            {
                cb.ToTable("ProducerLegalAddresses");
            });

            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);

            var companyTags = entity.Metadata.FindNavigation(nameof(Producer.Tags));
            companyTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasMany<DeliveryMode>().WithOne(c => c.Producer).HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<Returnable>().WithOne(c => c.Producer).HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
        }
    }
}
