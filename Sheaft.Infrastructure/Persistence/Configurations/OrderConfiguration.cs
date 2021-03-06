﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        private readonly bool _isAdmin;

        public OrderConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

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

            entity.HasMany(c => c.Products).WithOne().HasForeignKey(c => c.OrderId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity.HasMany(c => c.Deliveries).WithOne().HasForeignKey(c => c.OrderId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.PurchaseOrders).WithOne().HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.ToTable("Orders");
        }
    }
}