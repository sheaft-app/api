using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayinConfiguration : IEntityTypeConfiguration<Payin>
    {
        public void Configure(EntityTypeBuilder<Payin> entity)
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
            entity.HasOne(c => c.Order).WithMany().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(c => c.Refunds).WithOne(c => c.Payin).HasForeignKey("PayinUid").OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<WebPayin>(TransactionKind.WebPayin)
                .HasValue<PreAuthorizedPayin>(TransactionKind.PreAuthorizedPayin);

            var refunds = entity.Metadata.FindNavigation(nameof(Payin.Refunds));
            refunds.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("OrderUid");
            entity.HasIndex("AuthorUid");
            entity.HasIndex("CreditedWalletUid");
            entity.HasIndex("Uid", "Id", "AuthorUid", "OrderUid", "CreditedWalletUid", "RemovedOn");

            entity.ToTable("Payins");
        }
    }
}
