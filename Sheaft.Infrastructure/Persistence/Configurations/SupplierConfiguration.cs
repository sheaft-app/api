using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
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

        builder.OwnsOne(c => c.ShippingAddress);
        builder.OwnsOne(c => c.Legal, l =>
        {
            l.OwnsOne(le => le.Address);
            l.OwnsOne(le => le.Siret, s =>
            {
                s.Property(st => st.Value).HasColumnName("Legal_Siret");
                
                s.Ignore(st => st.Siren);
                s.Ignore(st => st.VatNumber);
                s.Ignore(st => st.NIC);
                
                s.HasIndex(st => st.Value).IsUnique();
            });
            
            l
                .Property(p => p.CorporateName)
                .HasConversion(name => name.Value, value => new CorporateName(value));
        });
         
        builder
            .Property(p => p.AccountIdentifier)
            .HasConversion(identifier => identifier.Value, value => new AccountId(value));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Supplier");
    }
}