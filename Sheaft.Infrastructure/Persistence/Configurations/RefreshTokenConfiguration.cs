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

        builder.Property<AccountId>("AccountId");

        builder
            .Property(p => p.Identifier)
            .HasMaxLength(36);

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();

        builder.ToTable("Account_RefreshTokens");
    }
}