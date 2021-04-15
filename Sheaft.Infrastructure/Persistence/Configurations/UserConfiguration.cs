using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
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
                .HasValue<Consumer>(ProfileKind.Consumer)
                .HasValue<Admin>(ProfileKind.Admin)
                .HasValue<Support>(ProfileKind.Support);

            entity.OwnsOne(c => c.Address, cb =>
            {
                cb.Property<long>("DepartmentUid");
                cb.HasOne(c => c.Department).WithMany().HasForeignKey("DepartmentUid");

                cb.ToTable("UserAddresses");
            });

            entity.OwnsMany(c => c.Points, p =>
            {
                p.Property<long>("Uid");
                p.HasKey("Uid");
                p.HasIndex(c => c.Id).IsUnique();
                p.ToTable("UserPoints");
            });

            entity.HasMany(c => c.Settings).WithOne().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Pictures).WithOne().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<Sponsoring>().WithOne(c => c.Sponsor).HasForeignKey("SponsorUid").OnDelete(DeleteBehavior.NoAction);

            var settings = entity.Metadata.FindNavigation(nameof(User.Settings));
            settings.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            var points = entity.Metadata.FindNavigation(nameof(User.Points));
            points.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            var pictures = entity.Metadata.FindNavigation(nameof(User.Pictures));
            pictures.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("Uid", "Id", "RemovedOn");

            entity.ToTable("Users");
        }
    }
}
