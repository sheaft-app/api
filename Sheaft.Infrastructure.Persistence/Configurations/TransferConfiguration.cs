﻿using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("AuthorUid");
            entity.Property<long>("PurchaseOrderUid");
            entity.Property<long>("CreditedWalletUid");
            entity.Property<long>("DebitedWalletUid");
            entity.Property<long?>("PayoutUid");
            entity.Property<long?>("RefundUid");

            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.HasOne(c => c.Author).WithMany().HasForeignKey("AuthorUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey("CreditedWalletUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey("DebitedWalletUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.PurchaseOrder).WithMany().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Refund).WithOne().HasForeignKey<Transfer>("RefundUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("AuthorUid");
            entity.HasIndex("PurchaseOrderUid");
            entity.HasIndex("CreditedWalletUid");
            entity.HasIndex("DebitedWalletUid");
            entity.HasIndex("PayoutUid");
            entity.HasIndex("RefundUid");
            entity.HasIndex("Uid", "Id", "AuthorUid", "PurchaseOrderUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn");

            entity.ToTable("Transfers");
        }
    }
}
