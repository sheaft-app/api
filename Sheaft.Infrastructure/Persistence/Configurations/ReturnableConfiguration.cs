using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class ReturnableConfiguration : IEntityTypeConfiguration<Returnable>
{
    public void Configure(EntityTypeBuilder<Returnable> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .Property(p => p.Name)
            .HasMaxLength(ReturnableName.MAXLENGTH)
            .HasConversion(name => name.Value, value => new ReturnableName(value));

        builder
            .Property(p => p.Reference)
            .HasMaxLength(ReturnableReference.MAXLENGTH)
            .HasConversion(reference => reference.Value, value => new ReturnableReference(value));

        builder
            .Property(p => p.UnitPrice)
            .HasPrecision(18, 2)
            .HasConversion(price => price.Value, value => new UnitPrice(value));
        
        builder
            .Property(p => p.Vat)
            .HasPrecision(18, 2)
            .HasConversion(vat => vat.Value, value => new VatRate(value));
        
        builder
            .Property(p => p.SupplierId)
            .HasMaxLength(Constants.IDS_LENGTH)
            .HasConversion(vat => vat.Value, value => new SupplierId(value));

        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .HasIndex(c => new { c.SupplierId, c.Reference})
            .IsUnique();
        
        builder.ToTable("Returnable");
    }
}