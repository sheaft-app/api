using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{

    public class RefundTransferTransactionConfiguration : IEntityTypeConfiguration<RefundTransferTransaction>
    {
        public void Configure(EntityTypeBuilder<RefundTransferTransaction> entity)
        {
            entity.Property<long>("PurchaseOrderUid");

            entity.HasOne(c => c.PurchaseOrder).WithMany().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex("PurchaseOrderUid");
        }
    }
}
