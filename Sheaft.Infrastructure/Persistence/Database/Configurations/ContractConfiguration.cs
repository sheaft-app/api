using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        private readonly bool _isAdmin;

        public ContractConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Contract> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);
            
            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity.HasOne(c => c.Distribution)
                .WithMany()
                .HasForeignKey(c => c.DistributionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Catalog)
                .WithMany()
                .HasForeignKey(c => c.CatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity.HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Contracts");
        }
    }
}