using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PreparedProductBatchConfiguration : IEntityTypeConfiguration<PreparedProductBatch>
    {
        public void Configure(EntityTypeBuilder<PreparedProductBatch> entity)
        {
            entity.HasOne(c => c.Batch).WithMany().HasForeignKey(c=>c.BatchId).OnDelete(DeleteBehavior.Restrict).IsRequired();

            entity.HasKey(c => new {c.BatchId, c.PreparedProductId});

            entity.ToTable("PreparedProductBatchs");
        }
    }
}