using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        private readonly bool _isAdmin;

        public TransferConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Transfer> entity)
        {
            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
entity.Property(c => c.RowVersion).IsRowVersion();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey(c=>c.AuthorId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey(c=>c.CreditedWalletId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey(c=>c.DebitedWalletId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            entity.HasOne(c => c.PurchaseOrder).WithMany().HasForeignKey(c=>c.PurchaseOrderId).OnDelete(DeleteBehavior.NoAction).IsRequired();

            entity.Ignore(c => c.DomainEvents);
            
            entity.HasKey(c=>c.Id);
            entity.HasIndex(c => c.Identifier);
            
            entity.ToTable("Transfers");
        }
    }
}
