using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsOne(d => d.Address, da =>
        {
            da.Property(b => b.Name)
                .HasMaxLength(TradeName.MAXLENGTH);
            
            da.Property(a => a.Email)
                .HasMaxLength(EmailAddress.MAXLENGTH);
            
            da.Property(a => a.Street)
                .HasMaxLength(Address.STREET_MAXLENGTH);
            
            da.Property(a => a.Complement)
                .HasMaxLength(Address.COMPLEMENT_MAXLENGTH);
            
            da.Property(a => a.Postcode)
                .HasMaxLength(Address.POSTCODE_MAXLENGTH);
            
            da.Property(a => a.City)
                .HasMaxLength(Address.CITY_MAXLENGTH);
        });
        
        builder
            .Property(p => p.Reference)
            .HasMaxLength(DeliveryReference.MAXLENGTH)
            .HasConversion(code => code != null ? code.Value : null, value => value != null ? new DeliveryReference(value) : null);
        
        builder
            .Property(p => p.ScheduledAt)
            .HasConversion(scheduledOn => scheduledOn.Value, value => new DeliveryDate(value, value));
        
        builder.OwnsMany(o => o.Lines, l =>
        {
            l.Property<long>("Id");
            l.HasKey("Id");
            
            l
                .Property(c => c.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH);

            l.Property(c => c.Reference)
                .HasMaxLength(Constants.LINE_REFERENCE_MAXLENGTH);
            
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
                    .HasConversion(unitPrice => unitPrice.Value, value => new OrderReference(value));
            });
            
            l.OwnsMany(ol => ol.Batches, b =>
            {
                b.Property<long>("DeliveryLineId");
                
                b
                    .Property(p => p.BatchIdentifier)
                    .HasMaxLength(Constants.IDS_LENGTH)
                    .HasConversion(batch => batch.Value, value => new BatchId(value));
            
                b.WithOwner().HasForeignKey("DeliveryLineId");
                b.HasKey("DeliveryLineId", "BatchIdentifier");

                b.ToTable("DeliveryLine_Batches");
            });
            
            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.Vat)
                .HasPrecision(18, 2)
                .HasConversion(vat => vat.Value, value => new VatRate(value));
            
            l.WithOwner().HasForeignKey("DeliveryId");
            l.ToTable("Delivery_Lines");
        });
        
        builder.OwnsMany(o => o.Adjustments, l =>
        {
            l.Property<long>("Id");
            l.HasKey("Id");
            
            l
                .Property(c => c.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH);
            
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
                    .HasConversion(unitPrice => unitPrice.Value, value => new OrderReference(value));
            });
            
            l.OwnsMany(ol => ol.Batches, b =>
            {
                b.Property<long>("DeliveryAdjustmentId");
                
                b
                    .Property(p => p.BatchIdentifier)
                    .HasMaxLength(Constants.IDS_LENGTH)
                    .HasConversion(batch => batch.Value, value => new BatchId(value));
            
                b.WithOwner().HasForeignKey("DeliveryAdjustmentId");
                b.HasKey("DeliveryAdjustmentId", "BatchIdentifier");

                b.ToTable("DeliveryAdjustment_Batches");
            });
            
            l
                .Property(c => c.Identifier)
                .HasMaxLength(Constants.IDS_LENGTH);
            
            l
                .Property(c => c.Reference)
                .HasMaxLength(Constants.LINE_REFERENCE_MAXLENGTH);
            
            l
                .Property(c => c.Name)
                .HasMaxLength(Constants.LINE_NAME_MAXLENGTH);

            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.Vat)
                .HasPrecision(18, 2)
                .HasConversion(vat => vat.Value, value => new VatRate(value));
            
            l.WithOwner().HasForeignKey("DeliveryId");

            l.ToTable("Delivery_Adjustments");
        });
        
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
        
        builder.ToTable("Delivery");
    }
}