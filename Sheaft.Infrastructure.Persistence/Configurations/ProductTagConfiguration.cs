using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.Property<long>("ProductUid");
            entity.Property<long>("TagUid");

            entity.HasOne(c => c.Tag).WithMany().HasForeignKey("TagUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("ProductUid", "TagUid");

            entity.ToTable("ProductTags");
        }
    }
}
