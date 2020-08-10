using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Sheaft.Interop.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long?>("CompanyUid");
            entity.Property<long?>("DepartmentUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Email).IsRequired();
            entity.Property(c => c.TotalPoints).HasDefaultValue(0);
            entity.Property(c => c.UserType).HasDefaultValue(UserKind.Consumer);

            entity.HasOne(c => c.Company).WithMany().HasForeignKey("CompanyUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Department).WithMany().HasForeignKey("DepartmentUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasMany<Sponsoring>().WithOne(c => c.Sponsor).HasForeignKey("SponsorUid").OnDelete(DeleteBehavior.NoAction);

            entity.OwnsMany(c => c.Points, p =>
            {
                p.Property<long>("Uid");
                p.HasKey("Uid");
                p.ToTable("UserPoints");
            });

            var points = entity.Metadata.FindNavigation(nameof(User.Points));
            points.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasIndex("CompanyUid");
            entity.HasIndex("DepartmentUid");
            entity.HasIndex("Uid", "Id", "CompanyUid", "DepartmentUid", "CreatedOn");

            entity.ToTable("Users");
        }
    }
}
