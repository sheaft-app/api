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
            l
                .Property(p => p.UnitPrice)
                .HasConversion(unitPrice => unitPrice.Value, value => new UnitPrice(value));

            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.TotalPrice)
                .HasConversion(totalPrice => totalPrice.Value, value => new TotalPrice(value));

            l
                .Property(p => p.Vat)
                .HasConversion(vat => vat.Value, value => new VatRate(value));

            l.WithOwner().HasForeignKey("InvoiceId");
            l.HasKey("InvoiceId", "Identifier");

            l.ToTable("Invoice_Lines");
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

        builder.ToTable("Invoice");
    }
}