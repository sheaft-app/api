using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderSenderConfiguration : IEntityTypeConfiguration<PurchaseOrderSender>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderSender> entity)
        {
            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();

            entity.HasKey(c=>c.Id);
            entity.ToTable("PurchaseOrderSenders");
        }
    }
}
