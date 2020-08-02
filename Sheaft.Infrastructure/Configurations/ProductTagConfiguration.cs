using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Sheaft.Infrastructure
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.Property<long>("ProductUid");
            entity.Property<long>("TagUid");

            entity.HasOne(c => c.Tag).WithMany().HasForeignKey("TagUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("ProductUid", "TagUid");

            entity.ToTable("ProductTags");
        }
    }
}
