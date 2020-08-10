using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ProducerUid");
            entity.Property<long?>("PackagingUid");

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
            entity.HasOne(c => c.Packaging).WithMany().HasForeignKey("PackagingUid").OnDelete(DeleteBehavior.NoAction);

            var productTags = entity.Metadata.FindNavigation(nameof(Product.Tags));
            productTags.SetPropertyAccessMode(PropertyAccessMode.Field);

            var productRatings = entity.Metadata.FindNavigation(nameof(Product.Ratings));
            productRatings.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("ProducerUid");
            entity.HasIndex("PackagingUid");
            entity.HasIndex("ProducerUid", "Reference").IsUnique();
            entity.HasIndex("Uid", "Id", "ProducerUid", "PackagingUid", "CreatedOn");

            entity.ToTable("Products");
        }
    }
}
