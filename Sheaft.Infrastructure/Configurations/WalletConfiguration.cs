using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid", "Kind").IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("Uid", "Id", "UserUid", "CreatedOn");

            entity.ToTable("Wallets");
        }
    }
}
