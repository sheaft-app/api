using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain.Enums;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class LegalConfiguration : IEntityTypeConfiguration<Legal>
    {
        public void Configure(EntityTypeBuilder<Legal> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.OwnsOne(c => c.Owner, e => {
                e.OwnsOne(a => a.Address);
            });

            entity.HasOne(c => c.User).WithOne().HasForeignKey<Legal>("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasDiscriminator<UserKind>("UserKind")
                .HasValue<ConsumerLegal>(UserKind.Consumer)
                .HasValue<BusinessLegal>(UserKind.Business);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("Uid", "Id");
            entity.HasIndex("UserUid");

            entity.ToTable("Legals");
        }
    }
}
