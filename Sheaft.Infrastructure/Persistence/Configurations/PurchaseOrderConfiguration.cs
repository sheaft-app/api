using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("OrderUid");
            entity.Property<long>("PurchaseOrderVendorUid");
            entity.Property<long>("PurchaseOrderSenderUid");
            entity.Property<long?>("TransferUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(o => o.Reference).IsRequired();

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

            entity.HasMany(o => o.Products).WithOne().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Vendor).WithOne().HasForeignKey<PurchaseOrder>("PurchaseOrderVendorUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Sender).WithOne().HasForeignKey<PurchaseOrder>("PurchaseOrderSenderUid").OnDelete(DeleteBehavior.Cascade);
            entity.OwnsOne(c => c.ExpectedDelivery, cb =>
            {
                cb.OwnsOne(ca => ca.Address);
                cb.ToTable("ExpectedDeliveries");
            });

            entity.Ignore(c => c.DomainEvents);
            
            var products = entity.Metadata.FindNavigation(nameof(PurchaseOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("PurchaseOrderVendorUid", "Reference").IsUnique();
            entity.HasIndex("OrderUid");
            entity.HasIndex("PurchaseOrderVendorUid");
            entity.HasIndex("PurchaseOrderSenderUid");
            entity.HasIndex("TransferUid");
            entity.HasIndex("OrderUid", "Uid", "Id", "PurchaseOrderVendorUid", "PurchaseOrderSenderUid", "RemovedOn");
            entity.ToTable("PurchaseOrders");
        }
    }
}
