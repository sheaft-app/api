using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class PurchaseOrderProductConfiguration : IEntityTypeConfiguration<PurchaseOrderProduct>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderProduct> entity)
        {
            entity.Property<long>("PurchaseOrderUid");

            entity.Property(c => c.Quantity).IsConcurrencyToken();

            entity.Property(o => o.TotalWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.UnitWeight).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalWeight).HasColumnType("decimal(10,2)");

            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.Reference).IsRequired();

            entity.Property(o => o.PackagingVat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.PackagingVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.PackagingWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.PackagingOnSalePrice).HasColumnType("decimal(10,2)");

            entity.HasKey("PurchaseOrderUid", "Id");

            entity.ToTable("PurchaseOrderProducts");
        }
    }
}
