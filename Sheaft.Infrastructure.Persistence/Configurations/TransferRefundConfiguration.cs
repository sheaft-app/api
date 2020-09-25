using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class TransferRefundConfiguration : IEntityTypeConfiguration<TransferRefund>
    {
        public void Configure(EntityTypeBuilder<TransferRefund> entity)
        {
            entity.Property<long>("CreditedWalletUid");
            entity.Property<long>("TransferUid");

            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey("CreditedWalletUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Transfer).WithMany().HasForeignKey("TransferUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("CreditedWalletUid");
            entity.HasIndex("TransferUid");
        }
    }
}
