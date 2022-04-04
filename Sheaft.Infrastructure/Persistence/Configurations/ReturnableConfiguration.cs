using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class ReturnableConfiguration : IEntityTypeConfiguration<Returnable>
{
    public void Configure(EntityTypeBuilder<Returnable> builder)
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
            .Property(p => p.Name)
            .HasConversion(name => name.Value, value => new ReturnableName(value));

        builder
            .Property(p => p.Reference)
            .HasConversion(reference => reference.Value, value => new ReturnableReference(value));

        builder
            .Property(p => p.Price)
            .HasConversion(price => price.Value, value => new Price(value));
        
        builder
            .Property(p => p.Vat)
            .HasConversion(vat => vat.Value, value => new VatRate(value));

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Returnable");
    }
}