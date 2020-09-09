using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Donation).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Fees).HasColumnType("decimal(10,2)");

            entity.HasMany(o => o.PurchaseOrders).WithOne().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(o => o.Transactions).WithOne().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            var purchaseOrders = entity.Metadata.FindNavigation(nameof(Order.PurchaseOrders));
            purchaseOrders.SetPropertyAccessMode(PropertyAccessMode.Field);

            var transactions = entity.Metadata.FindNavigation(nameof(Order.Transactions));
            transactions.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");

            entity.HasIndex("Uid", "Id", "UserUid", "CreatedOn");
            entity.ToTable("Orders");
        }
    }
}
