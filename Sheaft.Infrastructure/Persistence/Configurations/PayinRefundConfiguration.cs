using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayinRefundConfiguration : IEntityTypeConfiguration<PayinRefund>
    {
        public void Configure(EntityTypeBuilder<PayinRefund> entity)
        {
            entity.HasOne(c => c.PurchaseOrder).WithMany().HasForeignKey(c =>c.PurchaseOrderId).OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);
        }
    }
}
