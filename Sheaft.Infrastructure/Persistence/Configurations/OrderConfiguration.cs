using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long?>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            entity.Property(c => c.Donation).HasColumnName("Donate");
            entity.Property(c => c.DonationFeesPrice).HasColumnName("InternalFeesPrice");

            entity.Property(o => o.TotalPrice).HasColumnType("decimal(10,2)");
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

            entity.Property(o => o.Donation).HasColumnType("decimal(10,2)");
            entity.Property(o => o.FeesPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.FeesFixedAmount).HasColumnType("decimal(10,2)");
            entity.Property(o => o.FeesPercent).HasColumnType("decimal(10,4)");
            entity.Property(o => o.FeesVatPercent).HasColumnType("decimal(10,2)").HasDefaultValue(0);
            entity.Property(o => o.DonationFeesPrice).HasColumnType("decimal(10,2)");

            entity.HasMany(c => c.Products).WithOne().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Deliveries).WithOne().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.PurchaseOrders).WithOne().HasForeignKey("OrderUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.Ignore(c => c.DomainEvents);

            var purchaseOrders = entity.Metadata.FindNavigation(nameof(Order.PurchaseOrders));
            purchaseOrders.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            var products = entity.Metadata.FindNavigation(nameof(Order.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            var deliveries = entity.Metadata.FindNavigation(nameof(Order.Deliveries));
            deliveries.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "UserUid", "RemovedOn");
            entity.ToTable("Orders");
        }
    }
}
