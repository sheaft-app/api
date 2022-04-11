using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
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
        
        builder
            .Property(p => p.Number)
            .HasConversion(number => number.Value, value => new BatchNumber(value));

        builder
            .Property(p => p.SupplierIdentifier)
            .HasConversion(supplierIdentifier => supplierIdentifier.Value, value => new SupplierId(value));

        builder
            .Property(p => p.Date)
            .HasConversion(date => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc), value => DateOnly.FromDateTime(value));

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder
            .HasIndex(c => new {c.SupplierIdentifier, c.Number})
            .IsUnique();
        
        builder.ToTable("Batch");
    }
}