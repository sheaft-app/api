using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
    {
        private readonly bool _isAdmin;

        public AgreementConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Agreement> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Ignore(c => c.DomainEvents);

            entity.HasOne(c => c.Delivery)
                .WithMany()
                .HasForeignKey(c => c.DeliveryId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.Store)
                .WithMany()
                .HasForeignKey(c => c.StoreId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            entity.HasOne(c => c.Producer)
                .WithMany()
                .HasForeignKey(c => c.ProducerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasOne(c => c.Catalog)
                .WithMany()
                .HasForeignKey(c => c.CatalogId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasKey(c => c.Id);
            entity.ToTable("Agreements");
        }
    }
}