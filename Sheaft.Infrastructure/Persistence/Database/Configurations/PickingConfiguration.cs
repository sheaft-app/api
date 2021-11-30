using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class PickingConfiguration : IEntityTypeConfiguration<PickingOrder>
    {
        private readonly bool _isAdmin;

        public PickingConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<PickingOrder> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity.Ignore(c => c.DomainEvents);

            entity.OwnsMany(c => c.Products, p =>
            {
                p.OwnsMany(b => b.BatchNumbers, bn =>
                {
                    bn.HasIndex(c => c.BatchNumberId);
                    bn.ToTable("PickingOrderProductBatchNumbers");
                });

                p.HasIndex(c => c.ProductId);
                p.HasIndex(c => c.PurchaseOrderId);
                p.ToTable("PickingOrderProducts");
            });

            entity
                .HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            entity.HasKey(c => c.Id);
            entity.ToTable("PickingOrders");
        }
    }
}