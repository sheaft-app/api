using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
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

        builder.OwnsOne(c => c.DeliveryAddress);
        builder.OwnsOne(c => c.BillingAddress);
        
        builder.OwnsOne(c => c.Legal, l =>
        {
            l.OwnsOne(le => le.Address);

            l.Property(p => p.Siret)
                .HasConversion(siret => siret.Value, value => new Siret(value));
            
            l
                .Property(p => p.CorporateName)
                .HasConversion(name => name.Value, value => new CorporateName(value));
        });
         
        builder
            .Property(p => p.TradeName)
            .HasConversion(name => name.Value, value => new TradeName(value));
        
        builder
            .Property(p => p.AccountIdentifier)
            .HasConversion(identifier => identifier.Value, value => new AccountId(value));
        
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Customer");
    }
}