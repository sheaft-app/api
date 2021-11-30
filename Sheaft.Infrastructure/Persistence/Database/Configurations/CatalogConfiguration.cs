using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
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
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();            

            entity.Property(o => o.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();
            
            entity.HasMany(c => c.Products)
                .WithOne()
                .HasForeignKey(c => c.CatalogId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasKey(c => c.Id);
            entity.ToTable("Catalogs");
        }
    }

    public class CatalogProductPriceConfiguration : IEntityTypeConfiguration<CatalogProductPrice>
    {
        public void Configure(EntityTypeBuilder<CatalogProductPrice> entity)
        {
            entity.Ignore(o => o.VatPricePerUnit);
            entity.Ignore(o => o.OnSalePricePerUnit);
            entity.Ignore(o => o.VatPrice);
            entity.Ignore(o => o.OnSalePrice);

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity
                .Property(o => o.WholeSalePrice)
                .HasColumnType("decimal(10,2)");

            entity
                .Property(o => o.WholeSalePricePerUnit)
                .HasColumnType("decimal(10,2)");

            entity
                .HasOne(cp => cp.Product)
                .WithMany()
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("CatalogProductsPrices");
        }
    }
}