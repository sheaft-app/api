using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenInfo>
{
    public void Configure(EntityTypeBuilder<RefreshTokenInfo> builder)
    {
        builder
            .Property<long>("Id")
            .ValueGeneratedOnAdd();

        builder
            .HasKey("Id");

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

        builder.Property<long>("AccountId");

        builder
            .Property(p => p.Identifier)
            .HasMaxLength(36);

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();

        builder.ToTable("Account_RefreshTokens");
    }
}