using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PreAuthorizedPayinConfiguration : IEntityTypeConfiguration<PreAuthorizedPayin>
    {
        public void Configure(EntityTypeBuilder<PreAuthorizedPayin> entity)
        {
            entity.Property<long>("PreAuthorizationUid");
            entity.HasOne(c => c.PreAuthorization).WithMany().HasForeignKey("PreAuthorizationUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex("PreAuthorizationUid");
        }
    }
}
