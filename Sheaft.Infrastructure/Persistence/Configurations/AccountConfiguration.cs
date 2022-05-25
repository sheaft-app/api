using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class AccountConfiguration : IEntityTypeConfiguration<Account>
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
            .Property(p => p.Username)
            .HasMaxLength(EmailAddress.MAXLENGTH)
            .HasConversion(username => username.Value, value => new Username(value));
        
        builder
            .Property(p => p.Firstname)
            .HasMaxLength(Firstname.MAXLENGTH)
            .HasConversion(username => username.Value, value => new Firstname(value));
        
        builder
            .Property(p => p.Lastname)
            .HasMaxLength(Lastname.MAXLENGTH)
            .HasConversion(username => username.Value, value => new Lastname(value));

        builder.Property(b => b.Email)
            .HasMaxLength(EmailAddress.MAXLENGTH);
        
        builder
            .Property(c => c.Identifier)
            .HasMaxLength(Constants.IDS_LENGTH);
            
        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder
            .HasIndex(c => c.Username)
            .IsUnique();
        
        builder
            .HasIndex(c => c.Email)
            .IsUnique();
        
        builder.ToTable("Account");
    }
}