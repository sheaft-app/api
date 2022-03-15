using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
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

        builder.OwnsOne(c => c.Legal, innerBuilder =>
        {
            innerBuilder
                .Property(p => p.Name)
                .HasConversion(name => name.Value, value => new LegalName(value));

            innerBuilder
                .Property(p => p.Siret)
                .HasConversion(name => name.Value, value => new Siret(value));

            innerBuilder.OwnsOne(p => p.Address);
        });

        builder.OwnsOne(c => c.User);
        
        builder.Property<long>("AccountId");

        builder
            .Property(p => p.Identifier)
            .HasConversion(name => name.Value, value => new ProfileId(value));

        builder
            .Property(p => p.ContactEmail)
            .HasConversion(name => name.Value, value => new EmailAddress(value));

        builder
            .Property(p => p.Name)
            .HasConversion(name => name.Value, value => new CompanyName(value));

        builder
            .Property(p => p.ContactPhone)
            .HasConversion(name => name.Value, value => new PhoneNumber(value));

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Profiles");
    }
}