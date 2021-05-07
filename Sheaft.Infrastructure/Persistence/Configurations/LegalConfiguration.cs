using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class LegalConfiguration : IEntityTypeConfiguration<Legal>
    {
        public void Configure(EntityTypeBuilder<Legal> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            entity.OwnsOne(c => c.Owner, e =>
            {
                e.Property(c => c.RowVersion).IsRowVersion();
                e.OwnsOne(a => a.Address, a =>
                {
                    a.Property(ad => ad.RowVersion).IsRowVersion();
                });
            });

            entity.HasOne(c => c.User).WithOne(c => c.Legal).HasForeignKey<Legal>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasMany(c => c.Documents).WithOne().HasForeignKey(c => c.LegalId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasDiscriminator<UserKind>("UserKind")
                .HasValue<ConsumerLegal>(UserKind.Consumer)
                .HasValue<BusinessLegal>(UserKind.Business);

            entity.HasKey(c => c.Id);
            entity.ToTable("Legals");
        }
    }
}