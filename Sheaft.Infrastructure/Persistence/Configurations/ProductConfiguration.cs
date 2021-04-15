using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ProducerUid");
            entity.Property<long?>("ReturnableUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(o => o.Name).IsRequired();
            entity.Property(o => o.Reference).IsRequired();
            entity.Property(o => o.QuantityPerUnit).HasColumnType("decimal(10,3)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Weight).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Rating).HasColumnType("decimal(10,2)");
            entity.Property(o => o.RatingsCount).HasDefaultValue(0);

            entity.HasOne(c => c.Returnable).WithMany().HasForeignKey("ReturnableUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Producer).WithMany().HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Ratings).WithOne().HasForeignKey("ProductUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("ProductUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Pictures).WithOne().HasForeignKey("ProductUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.CatalogsPrices).WithOne(c => c.Product).HasForeignKey("ProductUid").OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);
            
            var tags = entity.Metadata.FindNavigation(nameof(Product.Tags));
            tags.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            var pictures = entity.Metadata.FindNavigation(nameof(Product.Pictures));
            pictures.SetPropertyAccessMode(PropertyAccessMode.Field);

            var ratings = entity.Metadata.FindNavigation(nameof(Product.Ratings));
            ratings.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            var prices = entity.Metadata.FindNavigation(nameof(Product.CatalogsPrices));
            prices.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("ProducerUid");
            entity.HasIndex("ReturnableUid");
            entity.HasIndex("ProducerUid", "Reference").IsUnique();
            entity.HasIndex("Uid", "Id", "ProducerUid", "ReturnableUid", "RemovedOn");

            entity.ToTable("Products");
        }
    }
}
