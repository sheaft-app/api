using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class RefundTransferTransactionConfiguration : IEntityTypeConfiguration<RefundTransferTransaction>
    {
        public void Configure(EntityTypeBuilder<RefundTransferTransaction> entity)
        {
            entity.Property<long>("CreditedWalletUid");
            entity.Property<long>("TransferTransactionUid");

            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey("CreditedWalletUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.TransferTransaction).WithMany().HasForeignKey("TransferTransactionUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("CreditedWalletUid");
            entity.HasIndex("TransferTransactionUid");
        }
    }
}
