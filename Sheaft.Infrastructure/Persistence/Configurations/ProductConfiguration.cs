using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        private readonly bool _isAdmin;

        public ProductConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn);
            entity.Property(c => c.RowVersion).IsRowVersion();

            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.RemovedOn.HasValue);

            entity.Property(o => o.Name).UseCollation("Latin1_general_CI_AI").IsRequired();
            entity.Property(o => o.Reference).IsRequired();
            entity.Property(o => o.QuantityPerUnit).HasColumnType("decimal(10,3)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Weight).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Rating).HasColumnType("decimal(10,2)");
            entity.Property(o => o.RatingsCount).HasDefaultValue(0);

            entity.HasOne(c => c.Returnable)
                .WithMany()
                .HasForeignKey(c => c.ReturnableId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.Producer)
                .WithMany()
                .HasForeignKey(c => c.ProducerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(c => c.Ratings)
                .WithOne()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(c => c.Tags)
                .WithOne()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(c => c.Pictures)
                .WithOne()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(c => c.CatalogsPrices)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Ignore(c => c.DomainEvents);

            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new {c.ProducerId, c.Reference}).IsUnique();

            entity.ToTable("Products");
        }
    }
}