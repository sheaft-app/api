using Microsoft.EntityFrameworkCore;
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
            .Property(p => p.Code)
            .HasConversion(code => code != null ? code.Value : null, value => value != null ? new DeliveryCode(value) : null);
        
        builder
            .Property(p => p.ScheduledAt)
            .HasConversion(scheduledOn => scheduledOn.Value, value => new DeliveryDate(value, value));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Delivery");
    }
}