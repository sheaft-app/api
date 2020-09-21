using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class PayoutTransactionConfiguration : IEntityTypeConfiguration<PayoutTransaction>
    {
        public void Configure(EntityTypeBuilder<PayoutTransaction> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("AuthorUid");
            entity.Property<long>("DebitedWalletUid");
            entity.Property<long>("BankAccountUid");

            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.HasOne(c => c.Author).WithMany().HasForeignKey("AuthorUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey("DebitedWalletUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.BankAccount).WithMany().HasForeignKey("BankAccountUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("AuthorUid");
            entity.HasIndex("BankAccountUid");
            entity.HasIndex("DebitedWalletUid");
            entity.HasIndex("Uid", "Id", "AuthorUid", "BankAccountUid", "DebitedWalletUid", "RemovedOn");

            entity.ToTable("PayoutTransactions");
        }
    }
}
