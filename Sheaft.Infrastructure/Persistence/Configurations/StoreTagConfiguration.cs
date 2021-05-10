using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class StoreTagConfiguration : IEntityTypeConfiguration<StoreTag>
    {
        public void Configure(EntityTypeBuilder<StoreTag> entity)
        {
            entity.HasOne(c => c.Tag).WithMany().HasForeignKey(c=>c.TagId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.HasKey(c=> new {c.StoreId, c.TagId});
            entity.ToTable("StoreTags");
        }
    }
}
