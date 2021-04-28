using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        private readonly bool _isAdmin;

        public RefundConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Refund> entity)
        {
            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey(c=>c.AuthorId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey(c=>c.DebitedWalletId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<PayinRefund>(TransactionKind.RefundPayin);

            entity.HasKey(c=>c.Id);
            entity.HasIndex(c => c.Identifier);

            entity.ToTable("Refunds");
        }
    }
}
