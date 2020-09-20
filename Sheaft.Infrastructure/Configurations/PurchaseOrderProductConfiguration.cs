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

            entity.Property(o => o.ReturnableVat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.ReturnableVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.ReturnableWholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.ReturnableOnSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalReturnableOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalReturnableWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.Property(o => o.TotalProductOnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductVatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.TotalProductWholeSalePrice).HasColumnType("decimal(10,2)");

            entity.HasKey("PurchaseOrderUid", "Id");

            entity.ToTable("PurchaseOrderProducts");
        }
    }
}
