using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        private readonly bool _isAdmin;

        public PurchaseOrderConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Reference).IsRequired();
            entity.Property(o => o.DroppedOn).HasColumnName("WithdrawnOn");

            entity.Property(o => o.TotalWeight).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalReturnableOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalProductOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.HasMany(o => o.Products).WithOne().HasForeignKey(c=>c.PurchaseOrderId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.HasOne(c => c.Vendor).WithOne().HasForeignKey<PurchaseOrder>(c=>c.VendorId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.Sender).WithOne().HasForeignKey<PurchaseOrder>(c=>c.SenderId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.OwnsOne(c => c.ExpectedDelivery, cb =>
            {
                cb.OwnsOne(ca => ca.Address);
            });

            entity.Ignore(c => c.DomainEvents);
            
            var products = entity.Metadata.FindNavigation(nameof(PurchaseOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey(c=>c.Id);

            entity.HasIndex(c=> new {PurchaseOrderVendorId = c.VendorId, c.Reference}).IsUnique();
            entity.ToTable("PurchaseOrders");
        }
    }
}
