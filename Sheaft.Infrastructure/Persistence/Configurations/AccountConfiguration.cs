using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.ProfileManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
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

        builder.OwnsOne(c => c.Password);
        builder.OwnsOne(c => c.ResetPasswordInfo);
        
        builder
            .HasMany(c => c.RefreshTokens)
            .WithOne()
            .HasForeignKey("AccountId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Navigation(c => c.RefreshTokens)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_refreshTokens");
        
        builder
            .HasOne(c => c.Profile)
            .WithOne()
            .HasForeignKey<Profile>("AccountId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Property(p => p.Username)
            .HasConversion(username => username.Value, value => new Username(value));
        
        builder
            .Property(p => p.Email)
            .HasConversion(email => email.Value, value => new EmailAddress(value));

        builder
            .HasIndex(c => c.Username)
            .IsUnique();
        
        builder
            .HasIndex(c => c.Email)
            .IsUnique();
        
        builder.ToTable("Accounts");
    }
}