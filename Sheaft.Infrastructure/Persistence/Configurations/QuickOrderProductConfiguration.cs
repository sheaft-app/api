using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class QuickOrderProductConfiguration : IEntityTypeConfiguration<QuickOrderProduct>
    {
        public void Configure(EntityTypeBuilder<QuickOrderProduct> entity)
        {
            entity.Property<long>("QuickOrderUid");
            entity.Property<long>("CatalogProductUid");
            entity.Property(c => c.Quantity).IsConcurrencyToken();

            entity.HasOne(c => c.CatalogProduct).WithMany().HasForeignKey("CatalogProductUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("QuickOrderUid", "CatalogProductUid");
            entity.ToTable("QuickOrderProducts");
        }
    }
}
