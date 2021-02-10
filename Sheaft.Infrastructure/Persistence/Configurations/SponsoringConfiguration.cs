using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class SponsoringConfiguration : IEntityTypeConfiguration<Sponsoring>
    {
        public void Configure(EntityTypeBuilder<Sponsoring> entity)
        {
            entity.Property<long>("SponsoredUid");
            entity.Property<long>("SponsorUid");

            entity.HasOne(c => c.Sponsored).WithMany().HasForeignKey("SponsoredUid").OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey("SponsorUid", "SponsoredUid");

            entity.ToTable("Sponsorings");
        }
    }
}
