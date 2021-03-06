﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayoutConfiguration : IEntityTypeConfiguration<Payout>
    {
        private readonly bool _isAdmin;

        public PayoutConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Payout> entity)
        {
            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
entity.Property(c => c.RowVersion).IsRowVersion();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey(c =>c.AuthorId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey(c =>c.DebitedWalletId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.BankAccount).WithMany().HasForeignKey(c =>c.BankAccountId).OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(c => c.Transfers).WithOne(c => c.Payout).HasForeignKey(c =>c.PayoutId).OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(c => c.Withholdings).WithOne(c => c.Payout).HasForeignKey(c =>c.PayoutId).OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Identifier);
            
            entity.ToTable("Payouts");
        }
    }
}
