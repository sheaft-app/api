using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BatchObservationConfiguration : IEntityTypeConfiguration<BatchObservation>
    {
        public void Configure(EntityTypeBuilder<BatchObservation> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);

            entity.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("BatchObservations");
        }
    }
}