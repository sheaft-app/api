using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class PayoutTransactionConfiguration : IEntityTypeConfiguration<PayoutTransaction>
    {
        public void Configure(EntityTypeBuilder<PayoutTransaction> entity)
        {
            entity.Property<long>("BankAccountUid");

            entity.HasOne(c => c.BankAccount).WithMany().HasForeignKey("BankAccountUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex("BankAccountUid");
        }
    }
}
