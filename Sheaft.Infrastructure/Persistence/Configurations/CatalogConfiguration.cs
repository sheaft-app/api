using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
    {
        private readonly bool _isAdmin;

        public CatalogConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }
        
        public void Configure(EntityTypeBuilder<Catalog> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();
            entity.Property(c => c.RemovedOn);
            
            if(!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            
            entity.HasOne(c => c.Producer).WithMany().HasForeignKey(c => c.ProducerId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            entity.HasMany(c => c.Products).WithOne(c => c.Catalog).HasForeignKey(c => c.CatalogId).OnDelete(DeleteBehavior.NoAction).IsRequired();
            
            var products = entity.Metadata.FindNavigation(nameof(Catalog.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey(c => c.Id);
            entity.ToTable("Catalogs");
        }
    }
}