using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Interop.Enums;

namespace Sheaft.Infrastructure
{
    public class PayinTransactionConfiguration : IEntityTypeConfiguration<PayinTransaction>
    {
        public void Configure(EntityTypeBuilder<PayinTransaction> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("AuthorUid");
            entity.Property<long>("OrderUid");
            entity.Property<long>("CreditedWalletUid");

            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.HasOne(c => c.Author).WithMany().HasForeignKey("AuthorUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey("CreditedWalletUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Order).WithMany(c => c.Transactions).HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<WebPayinTransaction>(TransactionKind.PayinWeb)
                .HasValue<CardPayinTransaction>(TransactionKind.PayinCard);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("OrderUid");
            entity.HasIndex("AuthorUid");
            entity.HasIndex("CreditedWalletUid");
            entity.HasIndex("Uid", "Id", "AuthorUid", "OrderUid", "CreditedWalletUid", "RemovedOn");

            entity.ToTable("PayinTransactions");
        }
    }
}
