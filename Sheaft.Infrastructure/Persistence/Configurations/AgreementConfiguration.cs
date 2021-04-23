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
            entity.Property<long>("Uid");
            entity.Property<long>("StoreUid");
            entity.Property<long>("ProducerUid");
            entity.Property<long?>("DeliveryModeUid");
            entity.Property<long?>("CatalogUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Ignore(c => c.DomainEvents);

            entity.HasOne(c => c.Delivery).WithMany().HasForeignKey("DeliveryModeUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Store).WithMany().HasForeignKey("StoreUid").OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            entity.HasOne(c => c.Producer).WithMany().HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity.HasOne(c => c.Catalog).WithMany().HasForeignKey("CatalogUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("StoreUid");
            entity.HasIndex("ProducerUid");
            entity.HasIndex("CatalogUid");
            entity.HasIndex("Uid", "Id", "StoreUid", "ProducerUid", "DeliveryModeUid", "CatalogUid", "RemovedOn");

            entity.ToTable("Agreements");
        }
    }
}