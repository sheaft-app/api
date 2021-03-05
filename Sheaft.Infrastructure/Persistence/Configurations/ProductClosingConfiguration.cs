﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class ProductClosingConfiguration : IEntityTypeConfiguration<ProductClosing>
    {
        public void Configure(EntityTypeBuilder<ProductClosing> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("ProductUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("ProductUid");
            entity.HasIndex("Uid", "Id", "ProductUid", "RemovedOn");

            entity.ToTable("ProductClosings");
        }
    }
}