using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RecallBatchConfiguration : IEntityTypeConfiguration<RecallBatch>
    {
        public void Configure(EntityTypeBuilder<RecallBatch> entity)
        {
            entity.Property(c => c.CreatedOn);

            entity.HasOne(c => c.Batch).WithMany().HasForeignKey(c => c.BatchId).OnDelete(DeleteBehavior.NoAction);
            
            entity.HasKey(c => new {c.BatchId, c.RecallId});
            entity.ToTable("RecallBatches");
        }
    }
}