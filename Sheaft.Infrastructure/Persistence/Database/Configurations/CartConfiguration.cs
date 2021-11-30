using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        private readonly bool _isAdmin;

        public CartConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Cart> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity.OwnsMany(c => c.Products, p =>
            {
                p.ToTable("CartProducts");
            });

            entity.HasKey(c => c.Id);
            entity.ToTable("Carts");
        }
    }
}