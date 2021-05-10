using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class SponsoringConfiguration : IEntityTypeConfiguration<Sponsoring>
    {
        public void Configure(EntityTypeBuilder<Sponsoring> entity)
        {
            entity.HasOne(c => c.Sponsored).WithMany().HasForeignKey(c=>c.SponsoredId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Sponsor).WithMany().HasForeignKey(c=>c.SponsorId).OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c=> new {c.SponsorId, c.SponsoredId});
            
            entity.ToTable("Sponsorings");
        }
    }
}
