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
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(c => c.Name).IsRequired();

            entity.HasMany(o => o.Products).WithOne().HasForeignKey(c=>c.QuickOrderId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasOne(c => c.User).WithMany().HasForeignKey(c=>c.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            entity.Ignore(c => c.DomainEvents);

            var products = entity.Metadata.FindNavigation(nameof(QuickOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey(c=>c.Id);
            entity.ToTable("QuickOrders");
        }
    }
}
