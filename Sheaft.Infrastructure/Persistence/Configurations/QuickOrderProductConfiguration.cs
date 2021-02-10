using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class QuickOrderProductConfiguration : IEntityTypeConfiguration<QuickOrderProduct>
    {
        public void Configure(EntityTypeBuilder<QuickOrderProduct> entity)
        {
            entity.Property<long>("QuickOrderUid");
            entity.Property<long>("ProductUid");

            entity.Property(c => c.Quantity).IsConcurrencyToken();

            entity.HasOne(c => c.Product).WithMany().HasForeignKey("ProductUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("QuickOrderUid", "ProductUid");

            entity.ToTable("QuickOrderProducts");
        }
    }
}
