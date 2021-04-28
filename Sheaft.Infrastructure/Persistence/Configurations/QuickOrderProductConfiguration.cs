using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class QuickOrderProductConfiguration : IEntityTypeConfiguration<QuickOrderProduct>
    {
        public void Configure(EntityTypeBuilder<QuickOrderProduct> entity)
        {
            entity.Property(c => c.Quantity).IsConcurrencyToken();

            entity.HasOne(c => c.CatalogProduct).WithMany().HasForeignKey(c=>c.CatalogProductId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.HasKey(c=> new {c.QuickOrderId, c.CatalogProductId});
            entity.ToTable("QuickOrderProducts");
        }
    }
}
