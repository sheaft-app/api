using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.OwnsMany(o => o.Lines, l =>
        {
            l.Property(c => c.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH);
            
            l
                .Property(p => p.Reference)
                .HasMaxLength(Constants.LINE_REFERENCE_MAXLENGTH);
            
            l
                .Property(p => p.Name)
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
            
            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.Vat)
                .HasPrecision(18, 2)
                .HasConversion(vat => vat.Value, value => new VatRate(value));
            
            l.WithOwner().HasForeignKey("OrderId");
            l.HasKey("OrderId", "Identifier");

            l.ToTable("Order_Lines");
        });
        
        builder
            .Property(p => p.Reference)
            .HasMaxLength(OrderReference.MAXLENGTH)
            .HasConversion(code => code != null ? code.Value : null, value => value != null ? new OrderReference(value) : null);

        builder
            .Property(p => p.InvoiceId)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .Property(p => p.DeliveryId)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .Property(p => p.TotalWholeSalePrice)
            .HasPrecision(18, 2)
            .HasConversion(totalPrice => totalPrice.Value, value => new TotalWholeSalePrice(value));
        
        builder
            .Property(p => p.TotalVatPrice)
            .HasPrecision(18, 2)
            .HasConversion(totalPrice => totalPrice.Value, value => new TotalVatPrice(value));
        
        builder
            .Property(p => p.TotalOnSalePrice)
            .HasPrecision(18, 2)
            .HasConversion(totalPrice => totalPrice.Value, value => new TotalOnSalePrice(value));
        
        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);

        builder
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(c => c.SupplierId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasIndex(c => new {c.SupplierId, c.Reference})
            .IsUnique();
        
        builder.ToTable("Order");
    }
}