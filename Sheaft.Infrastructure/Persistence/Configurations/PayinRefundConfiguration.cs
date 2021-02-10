using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayinRefundConfiguration : IEntityTypeConfiguration<PayinRefund>
    {
        public void Configure(EntityTypeBuilder<PayinRefund> entity)
        {
            entity.Property<long>("PayinUid");
            entity.Property<long>("PurchaseOrderUid");

            entity.HasOne(c => c.PurchaseOrder).WithMany().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);
            
            entity.HasIndex("PayinUid");
            entity.HasIndex("PurchaseOrderUid");
        }
    }
}
