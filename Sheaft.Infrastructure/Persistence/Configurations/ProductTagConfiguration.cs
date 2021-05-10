using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.HasOne(c => c.Tag).WithMany().HasForeignKey(c=>c.TagId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.HasKey(c => new {c.ProductId, c.TagId});
            entity.ToTable("ProductTags");
        }
    }
}
