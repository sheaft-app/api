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
            entity.Property<long>("PurchaseOrderVendorUid");
            entity.Property<long>("PurchaseOrderSenderUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(o => o.Reference).IsRequired();
            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalWeight).HasColumnType("decimal(10,2)");

            entity.HasMany(o => o.Products).WithOne().HasForeignKey("PurchaseOrderUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Vendor).WithOne().HasForeignKey<PurchaseOrder>("PurchaseOrderVendorUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Sender).WithOne().HasForeignKey<PurchaseOrder>("PurchaseOrderSenderUid").OnDelete(DeleteBehavior.Cascade);

            entity.OwnsOne(c => c.ExpectedDelivery, cb =>
            {
                cb.OwnsOne(ca => ca.Address, a =>
                {
                    a.ToTable("ExpectedDeliveryAddresses");
                });

                cb.ToTable("ExpectedDeliveries");
            });

            var products = entity.Metadata.FindNavigation(nameof(PurchaseOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("PurchaseOrderVendorUid", "Reference").IsUnique();
            entity.HasIndex("PurchaseOrderVendorUid");
            entity.HasIndex("PurchaseOrderSenderUid");

            entity.HasIndex("Uid", "Id", "PurchaseOrderVendorUid", "PurchaseOrderSenderUid", "CreatedOn");
            entity.ToTable("PurchaseOrders");
        }
    }
}
