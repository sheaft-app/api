﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsMany(o => o.Lines, l =>
        {
            l.Property<long>("Id");
            l.HasKey("Id");
            
            l.Property(c => c.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH);
            
            l.Property(c => c.Name)
                .HasMaxLength(Constants.LINE_NAME_MAXLENGTH);

            l.OwnsOne(ol => ol.PriceInfo, pi =>
            {
                pi
                    .Property(p => p.UnitPrice)
                    .HasPrecision(18, 2)
                    .HasConversion(unitPrice => unitPrice.Value, value => new UnitPrice(value));

                pi
                    .Property(p => p.WholeSalePrice)
                    .HasPrecision(18, 2)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineWholeSalePrice(value));

                pi
                    .Property(p => p.VatPrice)
                    .HasPrecision(18, 2)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineVatPrice(value));

                pi
                    .Property(p => p.OnSalePrice)
                    .HasPrecision(18, 2)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineOnSalePrice(value));
            });

            l.OwnsOne(ol => ol.Order, o =>
            {
                o
                    .Property(p => p.Reference)
                    .HasMaxLength(OrderReference.MAXLENGTH)
                    .HasConversion(value => value.Value, value => new OrderReference(value));
            });

            l.OwnsOne(ol => ol.Delivery, d =>
            {
                d
                    .Property(p => p.Reference)
                    .HasMaxLength(DeliveryReference.MAXLENGTH)
                    .HasConversion(value => value.Value, value => new DeliveryReference(value));
            });

            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.Vat)
                .HasPrecision(18, 2)
                .HasConversion(vat => vat.Value, value => new VatRate(value));

            l.WithOwner().HasForeignKey("InvoiceId");

            l.ToTable("Invoice_Lines");
        });

        builder.OwnsMany(o => o.Payments, l =>
        {
            l.WithOwner().HasForeignKey("InvoiceId");
            l.HasKey("InvoiceId", "Reference");

            l
                .Property(p => p.Reference)
                .HasMaxLength(PaymentReference.MAXLENGTH)
                .HasConversion(vat => vat.Value, value => new PaymentReference(value));

            l.ToTable("Invoice_Payments");
        });

        builder.OwnsOne(b => b.Customer, bi =>
        {
            bi
                .Property(p => p.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH)
                .HasConversion(identifier => identifier.Value, value => new CustomerId(value));

            bi
                .Property(p => p.Name)
                .HasMaxLength(TradeName.MAXLENGTH);
            
            bi
                .Property(p => p.Email)
                .HasMaxLength(EmailAddress.MAXLENGTH);
            
            bi
                .Property(p => p.Siret)
                .HasMaxLength(Siret.MAXLENGTH)
                .HasConversion(siret => siret.Value, value => new Siret(value));

            bi.OwnsOne(bie => bie.Address, da =>
            {
                da.Property(a => a.Street)
                    .HasMaxLength(Address.STREET_MAXLENGTH);
            
                da.Property(a => a.Complement)
                    .HasMaxLength(Address.COMPLEMENT_MAXLENGTH);
            
                da.Property(a => a.Postcode)
                    .HasMaxLength(Address.POSTCODE_MAXLENGTH);
            
                da.Property(a => a.City)
                    .HasMaxLength(Address.CITY_MAXLENGTH);
            });
        });

        builder.OwnsOne(b => b.Supplier, bi =>
        {
            bi
                .Property(p => p.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH)
                .HasConversion(identifier => identifier.Value, value => new SupplierId(value));

            bi
                .Property(p => p.Name)
                .HasMaxLength(TradeName.MAXLENGTH);
            
            bi
                .Property(p => p.Email)
                .HasMaxLength(EmailAddress.MAXLENGTH);
            
            bi
                .Property(p => p.Siret)
                .HasMaxLength(Siret.MAXLENGTH)
                .HasConversion(siret => siret.Value, value => new Siret(value));

            bi.OwnsOne(bie => bie.Address, da =>
            {
                da.Property(a => a.Street)
                    .HasMaxLength(Address.STREET_MAXLENGTH);
            
                da.Property(a => a.Complement)
                    .HasMaxLength(Address.COMPLEMENT_MAXLENGTH);
            
                da.Property(a => a.Postcode)
                    .HasMaxLength(Address.POSTCODE_MAXLENGTH);
            
                da.Property(a => a.City)
                    .HasMaxLength(Address.CITY_MAXLENGTH);
            });
        });
        
        builder
            .Property(p => p.Reference)
            .HasMaxLength(InvoiceReference.MAXLENGTH)
            .HasConversion(code => code != null ? code.Value : null,
                value => value != null ? new InvoiceReference(value) : null);

        builder
            .Property(p => p.DueDate)
            .HasConversion(dueOn => dueOn != null ? (DateTimeOffset?) dueOn.Value : null,
                value => value != null ? new InvoiceDueDate(value.Value) : null);

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
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);

        builder
            .HasMany(o => o.CreditNotes)
            .WithOne()
            .HasForeignKey("InvoiceId")
            .OnDelete(DeleteBehavior.NoAction);

        // builder
        //     .HasIndex("Supplier_Identifier", "Reference")
        //     .HasFilter("WHERE [Reference] IS NOT NULL")
        //     .IsUnique();

        builder.ToTable("Invoice");
    }
}