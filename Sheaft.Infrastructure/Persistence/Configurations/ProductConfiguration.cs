using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property<ReturnableId?>("ReturnableId");
        
        builder
            .Property(p => p.Name)
            .HasMaxLength(ProductName.MAXLENGTH)
            .HasConversion(name => name.Value, value => new ProductName(value));

        builder
            .Property(p => p.Reference)
            .HasMaxLength(ProductReference.MAXLENGTH)
            .HasConversion(code => code.Value, value => new ProductReference(value));

        builder
            .Property(p => p.Vat)
            .HasPrecision(18, 2)
            .HasConversion(vat => vat.Value, value => new VatRate(value));
        
        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);

        builder
            .HasOne(c => c.Returnable)
            .WithMany()
            .HasForeignKey("ReturnableId")
            .OnDelete(DeleteBehavior.NoAction);;
        
        builder
            .HasIndex(c => new {c.SupplierId, c.Reference})
            .IsUnique();
        
        builder.ToTable("Product");
    }
}