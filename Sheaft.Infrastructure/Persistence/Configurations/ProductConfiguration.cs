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
            entity.Property(o => o.VatPricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePricePerUnit).HasColumnType("decimal(10,2)");
            entity.Property(o => o.VatPrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.OnSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.WholeSalePrice).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Vat).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Weight).HasColumnType("decimal(10,2)");
            entity.Property(o => o.Rating).HasColumnType("decimal(10,2)");
            entity.Property(o => o.RatingsCount).HasDefaultValue(0);

            entity.HasOne(c => c.Producer).WithMany().HasForeignKey("ProducerUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Ratings).WithOne().HasForeignKey("ProductUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Tags).WithOne().HasForeignKey("ProductUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Returnable).WithMany().HasForeignKey("ReturnableUid").OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(c => c.DomainEvents);
            
            var productTags = entity.Metadata.FindNavigation(nameof(Product.Tags));
            productTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            var productRatings = entity.Metadata.FindNavigation(nameof(Product.Ratings));
            productRatings.SetPropertyAccessMode(PropertyAccessMode.Field);

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
