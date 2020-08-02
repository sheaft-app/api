using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Sheaft.Infrastructure
{
    public class PurchaseOrderVendorConfiguration : IEntityTypeConfiguration<PurchaseOrderVendor>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderVendor> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();

            entity.HasKey("Uid");
            entity.HasIndex("Id");

            entity.ToTable("PurchaseOrderVendors");
        }
    }
}
