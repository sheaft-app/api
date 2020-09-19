using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Interop.Enums;

namespace Sheaft.Infrastructure
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();
            entity.Property(c => c.FirstName).IsRequired();
            entity.Property(c => c.LastName).IsRequired();
            entity.Property(c => c.TotalPoints).HasDefaultValue(0);

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<Producer>(ProfileKind.Producer)
                .HasValue<Store>(ProfileKind.Store)
                .HasValue<Consumer>(ProfileKind.Consumer);

            entity.OwnsOne(c => c.Address, cb =>
            {
                cb.Property<long>("DepartmentUid");
                cb.HasOne(c => c.Department).WithMany().HasForeignKey("DepartmentUid");

                cb.ToTable("UserAddresses");
            });

            entity.HasMany<Sponsoring>().WithOne(c => c.Sponsor).HasForeignKey("SponsorUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasMany<Order>().WithOne(c => c.User).HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("Uid", "Id", "RemovedOn");

            entity.ToTable("Users");
        }
    }
}
