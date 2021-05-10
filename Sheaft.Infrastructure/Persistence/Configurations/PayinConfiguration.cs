using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PayinConfiguration : IEntityTypeConfiguration<Payin>
    {
        private readonly bool _isAdmin;

        public PayinConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Payin> entity)
        {
            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
entity.Property(c => c.RowVersion).IsRowVersion();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey(c =>c.AuthorId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey(c =>c.CreditedWalletId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.Order).WithMany().HasForeignKey(c =>c.OrderId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasMany(c => c.Refunds).WithOne(c => c.Payin).HasForeignKey(c =>c.PayinId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            
            entity.Ignore(c => c.DomainEvents);

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<WebPayin>(TransactionKind.WebPayin)
                .HasValue<PreAuthorizedPayin>(TransactionKind.PreAuthorizedPayin);

            entity.HasKey(c =>c.Id);
            entity.HasIndex(c => c.Identifier);
            entity.ToTable("Payins");
        }
    }
}
