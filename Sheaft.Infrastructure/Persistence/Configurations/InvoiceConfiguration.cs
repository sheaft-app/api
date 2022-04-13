﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
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
            l.Property<long>("Id");
            l.HasKey("Id");
            
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

            l.WithOwner().HasForeignKey("InvoiceId");

            l.ToTable("Invoice_Lines");
        });
        
        builder.OwnsMany(o => o.Vats, l =>
        {
            l
                .Property(p => p.Vat)
                .HasConversion(vatRate => vatRate.Value, value => new VatRate(value));

            l
                .Property(p => p.Price)
                .HasConversion(totalPrice => totalPrice.Value, value => new Price(value));

            l.WithOwner().HasForeignKey("InvoiceId");
            l.HasKey("InvoiceId", "Vat");

            l.ToTable("Invoice_Vats");
        });
        
        builder.OwnsMany(o => o.CreditNotes, l =>
        {
            l.HasKey("InvoiceId", "InvoiceIdentifier");
            l.ToTable("Invoice_CreditNotes");
        });

        builder.OwnsOne(b => b.BillingInformation, bi =>
        {
            bi
                .Property(p => p.Name)
                .HasConversion(name => name.Value, value => new TradeName(value));
            
            bi
                .Property(p => p.Siret)
                .HasConversion(siret => siret.Value, value => new Siret(value));

            bi.OwnsOne(bie => bie.Address);
        });

        builder
            .Property(p => p.Reference)
            .HasConversion(code => code != null ? code.Value : null,
                value => value != null ? new InvoiceReference(value) : null);

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
            .Property(p => p.CustomerIdentifier)
            .HasConversion(vat => vat.Value, value => new CustomerId(value));

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();

        builder
            .HasIndex(c => new {c.SupplierIdentifier, c.Reference})
            .IsUnique();

        builder.ToTable("Invoice");
    }
}