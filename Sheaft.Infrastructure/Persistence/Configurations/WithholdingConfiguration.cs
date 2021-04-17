﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class WithholdingConfiguration : IEntityTypeConfiguration<Withholding>
    {
        private readonly bool _isAdmin;

        public WithholdingConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Withholding> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("AuthorUid");
            entity.Property<long>("CreditedWalletUid");
            entity.Property<long>("DebitedWalletUid");
            entity.Property<long?>("PayoutUid");

            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey("AuthorUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey("CreditedWalletUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey("DebitedWalletUid").OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.Ignore(c => c.DomainEvents);
            
            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("AuthorUid");
            entity.HasIndex("CreditedWalletUid");
            entity.HasIndex("DebitedWalletUid");
            entity.HasIndex("PayoutUid");
            entity.HasIndex("Uid", "Id", "AuthorUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn");

            entity.ToTable("Withholdings");
        }
    }
}
