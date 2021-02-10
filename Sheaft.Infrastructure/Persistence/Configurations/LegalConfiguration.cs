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
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.OwnsOne(c => c.Owner, e => {
                e.OwnsOne(a => a.Address);
            });

            entity.OwnsMany(c => c.Documents, d => {
                d.Property(c => c.CreatedOn);
                d.Property(c => c.UpdatedOn).IsConcurrencyToken();

                d.OwnsMany(c => c.Pages, cb =>
                {
                    cb.Property(o => o.Filename).IsRequired();
                    cb.HasIndex(c => c.Id).IsUnique();

                    cb.ToTable("DocumentPages");
                });
                
                d.Ignore(c => c.DomainEvents);

                d.HasIndex(c => c.Id).IsUnique();
                d.HasIndex(c => c.Identifier);

                d.ToTable("Documents");
            });

            entity.HasOne(c => c.User).WithOne(u => u.Legal).HasForeignKey<Legal>("UserUid").OnDelete(DeleteBehavior.Cascade);

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
