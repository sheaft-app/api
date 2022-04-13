﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder
            .Property<long>("Id")
            .ValueGeneratedOnAdd();

        builder.HasKey("Id");
        
        builder
            .Property<DateTimeOffset>("CreatedOn")
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAdd();
        
        builder
            .Property<DateTimeOffset>("UpdatedOn")
            .HasDefaultValue(DateTimeOffset.UtcNow)
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAddOrUpdate();

        builder.OwnsOne(d => d.Address);
        
        builder.OwnsMany(d => d.Orders, o =>
        {
            o.WithOwner().HasForeignKey("DeliveryId");
            o.HasKey("DeliveryId", "OrderIdentifier");
            o.ToTable("Delivery_Orders");
        });
        
        builder
            .Property(p => p.Reference)
            .HasConversion(code => code != null ? code.Value : null, value => value != null ? new DeliveryReference(value) : null);
        
        builder
            .Property(p => p.ScheduledAt)
            .HasConversion(scheduledOn => scheduledOn.Value, value => new DeliveryDate(value, value));
        
        builder.OwnsMany(o => o.Lines, l =>
        {
            l.OwnsOne(ol => ol.PriceInfo, pi =>
            {
                pi
                    .Property(p => p.UnitPrice)
                    .HasConversion(unitPrice => unitPrice.Value, value => new UnitPrice(value));

                pi
                    .Property(p => p.Quantity)
                    .HasConversion(quantity => quantity.Value, value => new Quantity(value));

                pi
                    .Property(p => p.WholeSalePrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineWholeSalePrice(value));

                pi
                    .Property(p => p.VatPrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineVatPrice(value));

                pi
                    .Property(p => p.OnSalePrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineOnSalePrice(value));

                pi
                    .Property(p => p.Vat)
                    .HasConversion(vat => vat.Value, value => new VatRate(value));
            });
            
            l.WithOwner().HasForeignKey("DeliveryId");
            l.HasKey("DeliveryId", "Identifier");

            l.ToTable("Delivery_Lines");
        });
        
        builder.OwnsMany(b => b.Batches, b =>
        {
            b.WithOwner().HasForeignKey("DeliveryId");
            b.HasKey("DeliveryId", "BatchIdentifier", "ProductIdentifier");
                
            b.ToTable("Delivery_Batches");
        });
        
        builder.OwnsMany(o => o.Adjustments, l =>
        {
            l.OwnsOne(ol => ol.PriceInfo, pi =>
            {
                pi
                    .Property(p => p.UnitPrice)
                    .HasConversion(unitPrice => unitPrice.Value, value => new UnitPrice(value));

                pi
                    .Property(p => p.Quantity)
                    .HasConversion(quantity => quantity.Value, value => new Quantity(value));

                pi
                    .Property(p => p.WholeSalePrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineWholeSalePrice(value));

                pi
                    .Property(p => p.VatPrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineVatPrice(value));

                pi
                    .Property(p => p.OnSalePrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineOnSalePrice(value));

                pi
                    .Property(p => p.Vat)
                    .HasConversion(vat => vat.Value, value => new VatRate(value));
            });
            
            l.WithOwner().HasForeignKey("DeliveryId");
            l.HasKey("DeliveryId", "Identifier");

            l.ToTable("Delivery_Adjustments");
        });
        
        builder
            .Property(p => p.TotalWholeSalePrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new TotalWholeSalePrice(value));
        
        builder
            .Property(p => p.TotalVatPrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new TotalVatPrice(value));
        
        builder
            .Property(p => p.TotalOnSalePrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new TotalOnSalePrice(value));

        
        builder
            .Property(p => p.SupplierIdentifier)
            .HasConversion(vat => vat.Value, value => new SupplierId(value));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder
            .HasIndex(c => new {c.SupplierIdentifier, c.Reference})
            .IsUnique();
        
        builder.ToTable("Delivery");
    }
}