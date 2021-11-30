using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
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
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(u => u.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(c => c.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity
                .Property(c => c.Email)
                .IsRequired();

            entity
                .Property(c => c.Firstname)
                .IsRequired();

            entity
                .Property(c => c.Lastname)
                .IsRequired();

            entity.OwnsOne(c => c.Address);

            entity
                .HasMany(c => c.Carts)
                .WithOne()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Email).IsUnique();
            entity.ToTable("Users");
        }
    }
}