using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class QuickOrderProductConfiguration : IEntityTypeConfiguration<QuickOrderProduct>
    {
        public void Configure(EntityTypeBuilder<QuickOrderProduct> entity)
        {
            entity.Property(c => c.Quantity);
            entity.Property(c => c.RowVersion).IsRowVersion();

            entity.HasOne(c => c.CatalogProduct).WithMany().HasForeignKey(c=>c.CatalogProductId).OnDelete(DeleteBehavior.Cascade);

            entity.HasKey(c => c.Id);
            entity.HasIndex(c=> new {c.QuickOrderId, c.CatalogProductId}).IsUnique();
            entity.ToTable("QuickOrderProducts");
        }
    }
}
