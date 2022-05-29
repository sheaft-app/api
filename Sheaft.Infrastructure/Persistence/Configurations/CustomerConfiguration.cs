using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.CustomerManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

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
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(c => c.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.ToTable("Customer");
    }
}