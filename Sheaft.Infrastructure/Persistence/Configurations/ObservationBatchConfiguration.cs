using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ObservationBatchConfiguration : IEntityTypeConfiguration<ObservationBatch>
    {
        public void Configure(EntityTypeBuilder<ObservationBatch> entity)
        {
            entity.Property(c => c.CreatedOn);

            entity.HasOne(c => c.Batch).WithMany().HasForeignKey(c => c.BatchId).OnDelete(DeleteBehavior.NoAction);
            
            entity.HasKey(c => new {c.BatchId, c.ObservationId});
            entity.ToTable("ObservationBatches");
        }
    }
}