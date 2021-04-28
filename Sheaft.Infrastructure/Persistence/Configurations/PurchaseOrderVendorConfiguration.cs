using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderVendorConfiguration : IEntityTypeConfiguration<PurchaseOrderVendor>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderVendor> entity)
        {
            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();

            entity.HasKey(c=>c.Id);
            entity.ToTable("PurchaseOrderVendors");
        }
    }
}
