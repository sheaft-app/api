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

        builder.Property(b => b.Email)
            .HasMaxLength(EmailAddress.MAXLENGTH);

        builder.Property(b => b.Phone)
            .HasMaxLength(PhoneNumber.MAXLENGTH);
        
        builder.Property(b => b.TradeName)
            .HasMaxLength(TradeName.MAXLENGTH);
        
        builder.OwnsOne(c => c.DeliveryAddress, da =>
        {
            da.Property(b => b.Name)
                .HasMaxLength(TradeName.MAXLENGTH);
            
            da.Property(a => a.Email)
                .HasMaxLength(EmailAddress.MAXLENGTH);
            
            da.Property(a => a.Street)
                .HasMaxLength(Address.STREET_MAXLENGTH);
            
            da.Property(a => a.Complement)
                .HasMaxLength(Address.COMPLEMENT_MAXLENGTH);
            
            da.Property(a => a.Postcode)
                .HasMaxLength(Address.POSTCODE_MAXLENGTH);
            
            da.Property(a => a.City)
                .HasMaxLength(Address.CITY_MAXLENGTH);
        });
        
        builder.OwnsOne(c => c.BillingAddress, da =>
        {
            da.Property(b => b.Name)
                .HasMaxLength(TradeName.MAXLENGTH);
            
            da.Property(a => a.Email)
                .HasMaxLength(EmailAddress.MAXLENGTH);
            
            da.Property(a => a.Street)
                .HasMaxLength(Address.STREET_MAXLENGTH);
            
            da.Property(a => a.Complement)
                .HasMaxLength(Address.COMPLEMENT_MAXLENGTH);
            
            da.Property(a => a.Postcode)
                .HasMaxLength(Address.POSTCODE_MAXLENGTH);
            
            da.Property(a => a.City)
                .HasMaxLength(Address.CITY_MAXLENGTH);
        });
        
        builder.OwnsOne(c => c.Legal, l =>
        {
            l.OwnsOne(le => le.Address, da =>
            {
                da.Property(a => a.Street)
                    .HasMaxLength(Address.STREET_MAXLENGTH);
            
                da.Property(a => a.Complement)
                    .HasMaxLength(Address.COMPLEMENT_MAXLENGTH);
            
                da.Property(a => a.Postcode)
                    .HasMaxLength(Address.POSTCODE_MAXLENGTH);
            
                da.Property(a => a.City)
                    .HasMaxLength(Address.CITY_MAXLENGTH);
            });

            l.Property(p => p.Siret)
                .HasMaxLength(Siret.MAXLENGTH)
                .HasConversion(siret => siret.Value, value => new Siret(value));
            
            l
                .Property(p => p.CorporateName)
                .HasMaxLength(CorporateName.MAXLENGTH)
                .HasConversion(name => name.Value, value => new CorporateName(value));
        });
         
        builder
            .Property(p => p.TradeName)
            .HasConversion(name => name.Value, value => new TradeName(value));
        
        builder
            .Property(p => p.AccountIdentifier)
            .HasMaxLength(Constants.IDS_LENGTH)
            .HasConversion(identifier => identifier.Value, value => new AccountId(value));
        
        builder
            .Property(c => c.Identifier)
            .HasMaxLength(Constants.IDS_LENGTH);
            
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Customer");
    }
}