using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class StoreTagConfiguration : IEntityTypeConfiguration<StoreTag>
    {
        public void Configure(EntityTypeBuilder<StoreTag> entity)
        {
            entity.Property<long>("StoreUid");
            entity.Property<long>("TagUid");

            entity.HasOne(c => c.Tag).WithMany().HasForeignKey("TagUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("StoreUid", "TagUid");

            entity.ToTable("StoreTags");
        }
    }
}
