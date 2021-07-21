using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RecallProductConfiguration : IEntityTypeConfiguration<RecallProduct>
    {
        public void Configure(EntityTypeBuilder<RecallProduct> entity)
        {
            entity.Property(c => c.CreatedOn);

            entity.HasOne(c => c.Product).WithMany().HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.NoAction);

            entity.HasKey(c => c.Id);
            entity.ToTable("RecallProducts");
        }
    }
}