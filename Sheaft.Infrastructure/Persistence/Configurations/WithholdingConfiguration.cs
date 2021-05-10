using Microsoft.EntityFrameworkCore;
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
            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Credited).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Debited).HasColumnType("decimal(10,2)");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            entity.HasOne(c => c.CreditedWallet).WithMany().HasForeignKey(c => c.CreditedWalletId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.DebitedWallet).WithMany().HasForeignKey(c => c.DebitedWalletId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Identifier);

            entity.ToTable("Withholdings");
        }
    }
}