using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain.Enums;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class LegalConfiguration : IEntityTypeConfiguration<Legal>
    {
        public void Configure(EntityTypeBuilder<Legal> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.OwnsOne(c => c.Owner, e => {
                e.OwnsOne(a => a.Address);
            });

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<ConsumerLegal>(LegalKind.Natural)
                .HasValue<BusinessLegal>(LegalKind.Business)
                .HasValue<BusinessLegal>(LegalKind.Individual)
                .HasValue<BusinessLegal>(LegalKind.Organization);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("Uid", "Id");

            entity.ToTable("Legals");
        }
    }
}
