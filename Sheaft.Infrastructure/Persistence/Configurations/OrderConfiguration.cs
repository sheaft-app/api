using Microsoft.EntityFrameworkCore;
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

        builder.OwnsOne(o => o.BillingAddress);
        builder.OwnsOne(o => o.DeliveryAddress);
        
        builder.OwnsMany(o => o.Lines, l =>
        {
            l
                .Property(p => p.Code)
                .HasConversion(code => code.Value, value => new ProductCode(value));
            
            l
                .Property(p => p.Name)
                .HasConversion(name => name.Value, value => new ProductName(value));

            l
                .Property(p => p.UnitPrice)
                .HasConversion(unitPrice => unitPrice.Value, value => new Price(value));

            l
                .Property(p => p.Quantity)
                .HasConversion(quantity => quantity.Value, value => new Quantity(value));

            l
                .Property(p => p.TotalPrice)
                .HasConversion(totalPrice => totalPrice.Value, value => new Price(value));

            l
                .Property(p => p.Vat)
                .HasConversion(vat => vat.Value, value => new VatRate(value));
            
            l.WithOwner().HasForeignKey("OrderId");
            l.HasKey("OrderId", "ProductIdentifier");

            l.ToTable("Order_Lines");
        });
        
        builder
            .Property(p => p.Code)
            .HasConversion(code => code.Value, value => new OrderCode(value));
        
        builder
            .Property(p => p.TotalPrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new Price(value));
        
        builder
            .Property(p => p.DeliveryDate)
            .HasConversion(deliveryDate => deliveryDate.Value, value => new OrderDeliveryDate(new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, DateTimeKind.Utc), new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(-1)));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Order");
    }
}