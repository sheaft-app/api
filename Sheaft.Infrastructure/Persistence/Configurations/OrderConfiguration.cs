﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
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
        
        builder.OwnsMany(o => o.Lines, l =>
        {
            l
                .Property(p => p.UnitPrice)
                .HasConversion(unitPrice => unitPrice.Value, value => new UnitPrice(value));

            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new OrderedQuantity(value));

            l
                .Property(p => p.TotalPrice)
                .HasConversion(totalPrice => totalPrice.Value, value => new TotalPrice(value));

            l
                .Property(p => p.Vat)
                .HasConversion(vat => vat.Value, value => new VatRate(value));
            
            l.WithOwner().HasForeignKey("OrderId");
            l.HasKey("OrderId", "Identifier");

            l.ToTable("Order_Lines");
        });
        
        builder
            .Property(p => p.Reference)
            .HasConversion(code => code != null ? code.Value : null, value => value != null ? new OrderReference(value) : null);
        
        builder
            .Property(p => p.TotalPrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new UnitPrice(value));
        
        builder
            .Property(p => p.SupplierIdentifier)
            .HasConversion(vat => vat.Value, value => new SupplierId(value));
        
        builder
            .Property(p => p.CustomerIdentifier)
            .HasConversion(vat => vat.Value, value => new CustomerId(value));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder
            .HasIndex(c => new {c.SupplierIdentifier, c.Reference})
            .IsUnique();
        
        builder.ToTable("Order");
    }
}