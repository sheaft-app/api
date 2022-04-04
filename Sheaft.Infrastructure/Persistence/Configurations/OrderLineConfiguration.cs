using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder
            .Property(p => p.UnitPrice)
            .HasConversion(unitPrice => unitPrice.Value, value => new Price(value));

        builder
            .Property(p => p.Quantity)
            .HasConversion(quantity => quantity.Value, value => new Quantity(value));

        builder
            .Property(p => p.TotalPrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new Price(value));

        builder
            .Property(p => p.Vat)
            .HasConversion(vat => vat.Value, value => new VatRate(value));
        
        builder
            .Property(p => p.TotalPrice)
            .HasConversion(totalPrice => totalPrice.Value, value => new Price(value));
        
        builder.ToTable("Order_Lines");
    }
}