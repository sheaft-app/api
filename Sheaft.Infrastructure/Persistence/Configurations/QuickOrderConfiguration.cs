using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class QuickOrderConfiguration : IEntityTypeConfiguration<QuickOrder>
    {
        private readonly bool _isAdmin;

        public QuickOrderConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<QuickOrder> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).IsRequired();

            entity.HasMany(o => o.Products).WithOne().HasForeignKey("QuickOrderUid").OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.Ignore(c => c.DomainEvents);

            var products = entity.Metadata.FindNavigation(nameof(QuickOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "UserUid", "RemovedOn");

            entity.ToTable("QuickOrders");
        }
    }
}
