using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly bool _isAdmin;

        public UserConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
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

            entity.OwnsOne(c => c.Address,
                cb => { cb.HasOne(c => c.Department).WithMany().HasForeignKey(c => c.DepartmentId); });

            entity.HasMany(c => c.Points).WithOne().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Settings).WithOne().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity.HasMany(c => c.Pictures).WithOne().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasIndex(c => c.Identifier);

            entity.ToTable("Users");
        }
    }
}