using Microsoft.EntityFrameworkCore;
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
                    .Property(p => p.WholeSalePrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineWholeSalePrice(value));

                pi
                    .Property(p => p.VatPrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineVatPrice(value));

                pi
                    .Property(p => p.OnSalePrice)
                    .HasConversion(totalPrice => totalPrice.Value, value => new LineOnSalePrice(value));
            });
            
            l.OwnsOne(ol => ol.Order, o =>
            {
                o
                    .Property(p => p.Reference)
                    .HasConversion(value => value.Value, value => new OrderReference(value));
            });
            
            l.OwnsOne(ol => ol.Delivery, d =>
            {
                d
                    .Property(p => p.Reference)
                    .HasConversion(value => value.Value, value => new DeliveryReference(value));
            });
            
            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.Vat)
                .HasConversion(vat => vat.Value, value => new VatRate(value));

            l.WithOwner().HasForeignKey("InvoiceId");

            l.ToTable("Invoice_Lines");
        });
        
        builder.OwnsMany(o => o.Payments, l =>
        {
            l.WithOwner().HasForeignKey("InvoiceId");
            l.HasKey("InvoiceId", "Reference");

            l.ToTable("Invoice_Payments");
        });
        
        builder.OwnsMany(o => o.CreditNotes, l =>
        {
            l.HasKey("InvoiceId", "InvoiceIdentifier");
            l.ToTable("Invoice_CreditNotes");
        });

        builder.OwnsOne(b => b.Customer, bi =>
        {
            bi
                .Property(p => p.Identifier)
                .HasConversion(identifier => identifier.Value, value => new CustomerId(value));

            bi
                .Property(p => p.Siret)
                .HasConversion(siret => siret.Value, value => new Siret(value));

            bi.OwnsOne(bie => bie.Address);
        });

        builder.OwnsOne(b => b.Supplier, bi =>
        {
            bi
                .Property(p => p.Identifier)
                .HasConversion(identifier => identifier.Value, value => new SupplierId(value));

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
            .Property(p => p.DueDate)
            .HasConversion(dueOn => dueOn != null ? (DateTimeOffset?)dueOn.Value : null,
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
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        // builder
        //     .HasIndex("Supplier_Identifier", "Reference")
        //     .IsUnique();

        builder.ToTable("Invoice");
    }
}