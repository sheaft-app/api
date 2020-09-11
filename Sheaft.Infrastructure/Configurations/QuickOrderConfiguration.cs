using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure
{
    public class QuickOrderConfiguration : IEntityTypeConfiguration<QuickOrder>
    {
        public void Configure(EntityTypeBuilder<QuickOrder> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long>("UserUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();

            entity.HasMany(o => o.Products).WithOne().HasForeignKey("QuickOrderUid").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            var products = entity.Metadata.FindNavigation(nameof(QuickOrder.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("Uid", "Id", "UserUid", "RemovedOn");

            entity.ToTable("QuickOrders");
        }
    }
}
