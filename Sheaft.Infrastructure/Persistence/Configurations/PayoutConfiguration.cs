using Microsoft.EntityFrameworkCore;
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
            entity.Property<long>("Uid");
            entity.Property<long>("AuthorUid");
            entity.Property<long>("DebitedWalletUid");
            entity.Property<long>("BankAccountUid");

            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey("AuthorUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey("DebitedWalletUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.BankAccount).WithMany().HasForeignKey("BankAccountUid").OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasMany(c => c.Transfers).WithOne(c => c.Payout).HasForeignKey("PayoutUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(c => c.Withholdings).WithOne(c => c.Payout).HasForeignKey("PayoutUid").OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);

            var transfers = entity.Metadata.FindNavigation(nameof(Payout.Transfers));
            transfers.SetPropertyAccessMode(PropertyAccessMode.Field);

            var withholdings = entity.Metadata.FindNavigation(nameof(Payout.Withholdings));
            withholdings.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("AuthorUid");
            entity.HasIndex("BankAccountUid");
            entity.HasIndex("DebitedWalletUid");
            entity.HasIndex("Uid", "Id", "AuthorUid", "BankAccountUid", "DebitedWalletUid", "RemovedOn");

            entity.ToTable("Payouts");
        }
    }
}
