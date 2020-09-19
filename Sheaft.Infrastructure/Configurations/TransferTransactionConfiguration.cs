using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class TransferTransactionConfiguration : IEntityTypeConfiguration<TransferTransaction>
    {
        public void Configure(EntityTypeBuilder<TransferTransaction> entity)
        {
            entity.Property<long>("PurchaseOrderUid");

            entity.HasOne(c => c.PurchaseOrder).WithMany(c => c.Transactions).HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("PurchaseOrderUid");
        }
    }
}
