using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class SponsoringConfiguration : IEntityTypeConfiguration<Sponsoring>
    {
        public void Configure(EntityTypeBuilder<Sponsoring> entity)
        {
            entity.Property<long>("SponsoredUid");
            entity.Property<long>("SponsorUid");

            entity.HasOne(c => c.Sponsored).WithMany().HasForeignKey("SponsoredUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("SponsorUid", "SponsoredUid");

            entity.ToTable("Sponsorings");
        }
    }
}
