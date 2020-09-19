using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("OrderUid");
            entity.Property<long>("PurchaseOrderVendorUid");
            entity.Property<long>("PurchaseOrderSenderUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(o => o.Reference).IsRequired();

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalReturnableOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalWeight).HasColumnType("decimal(10,2)");

            entity.HasMany(o => o.Products).WithOne().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Vendor).WithOne().HasForeignKey<PurchaseOrder>("PurchaseOrderVendorUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Sender).WithOne().HasForeignKey<PurchaseOrder>("PurchaseOrderSenderUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Order).WithMany().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);

            entity.OwnsOne(c => c.ExpectedDelivery, cb =>
            {
                cb.OwnsOne(ca => ca.Address);
                cb.ToTable("ExpectedDeliveries");
            });

            var products = entity.Metadata.FindNavigation(nameof(PurchaseOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            var transactions = entity.Metadata.FindNavigation(nameof(PurchaseOrder.Transactions));
            transactions.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("PurchaseOrderVendorUid", "Reference").IsUnique();
            entity.HasIndex("OrderUid");
            entity.HasIndex("PurchaseOrderVendorUid");
            entity.HasIndex("PurchaseOrderSenderUid");

            entity.HasIndex("OrderUid", "Uid", "Id", "PurchaseOrderVendorUid", "PurchaseOrderSenderUid", "RemovedOn");
            entity.ToTable("PurchaseOrders");
        }
    }
}
