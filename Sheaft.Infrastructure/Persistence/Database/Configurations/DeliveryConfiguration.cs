using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        private readonly bool _isAdmin;

        public DeliveryConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Delivery> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity.OwnsOne(o => o.Address);

            entity.OwnsMany(c => c.Products, p =>
            {
                p.ToTable("DeliveryProducts");
            });

            entity.OwnsMany(c => c.PickedUpReturnables, p =>
            {
                p.ToTable("DeliveryPickedUpReturnables");
            });

            entity
                .HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new { c.SupplierId, c.Reference }).IsUnique();

            entity.ToTable("Deliveries");
        }
    }
}