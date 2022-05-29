using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder
            .Property(p => p.Number)
            .HasMaxLength(40)
            .HasConversion(number => number.Value, value => new BatchNumber(value));

        builder
            .Property(p => p.Date)
            .HasConversion(date => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc), value => DateOnly.FromDateTime(value));

        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .HasIndex(c => new {c.SupplierId, c.Number})
            .IsUnique();
        
        builder.ToTable("Batch");
    }
}