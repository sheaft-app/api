using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property<long>("Id")
            .ValueGeneratedOnAdd();
        
        builder.HasKey("Id");
        
        builder.Property<DateTimeOffset>("CreatedOn")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAdd();
        
        builder.Property<DateTimeOffset>("UpdatedOn")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAddOrUpdate();

        builder.Property<long>("AccountId");
        
        builder.Property(p => p.Identifier)
            .HasConversion(name => name.Value, value => new RefreshTokenId(value));

        builder.HasIndex(c => c.Identifier).IsUnique();
        
        builder.ToTable("RefreshTokens");
    }
}