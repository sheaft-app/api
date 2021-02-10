using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ReturnableConfiguration : IEntityTypeConfiguration<Returnable>
    {
        public void Configure(EntityTypeBuilder<Returnable> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ProducerUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Kind).HasDefaultValue(ReturnableKind.Container);

            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.VatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("ProducerUid");
            entity.HasIndex("Uid", "Id", "RemovedOn");

            entity.ToTable("Returnables");
        }
    }
}
